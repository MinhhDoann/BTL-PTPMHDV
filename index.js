
function showModule(id) {
  document.querySelectorAll(".module-content").forEach((m) => m.classList.remove("active"));
  const el = document.getElementById(id);
  if (el) el.classList.add("active");

  // luôn load dữ liệu khi chuyển module (không phụ thuộc menu là <a> hay <button>)
  setTimeout(() => {
    try {
      loadByModuleId(id);
    } catch (e) {
      console.error("showModule load error:", e);
    }
  }, 0);
}

const API_BASE = "http://localhost:28171";

const ENTITIES = {
  customers: {
    label: "Khách hàng",
    moduleIds: ["customers", "khachhang", "khach-hang"],
    apiPrefix: "/api/khach-hang",
    idField: "khachHangID",
  },
  contracts: {
    label: "Hợp đồng",
    moduleIds: ["contracts", "hopdong", "hop-dong"],
    apiPrefix: "/api/hop-dong",
    idField: "hopDongID",
  },
  costs: {
    label: "Chi phí",
    moduleIds: ["costs", "chiphi", "chi-phi"],
    apiPrefix: "/api/chi-phi",
    idField: "chiPhiID",
  },
  invoices: {
    label: "Hóa đơn",
    moduleIds: ["invoices", "hoadon", "hoa-don"],
    apiPrefix: "/api/hoa-don",
    idField: "hoaDonID",
  },
  payments: {
    label: "Thanh toán",
    moduleIds: ["payments", "thanhtoan", "thanh-toan"],
    apiPrefix: "/api/thanh-toan",
    idField: "thanhToanID",
  },
};

function findEntityKeyByModuleId(moduleId) {
  const id = (moduleId || "").toLowerCase();
  return Object.keys(ENTITIES).find((k) => ENTITIES[k].moduleIds.map((x) => x.toLowerCase()).includes(id)) || null;
}

function toISODate(value) {
  if (!value) return "";
  const d = new Date(value);
  if (Number.isNaN(d.getTime())) return String(value).slice(0, 10);
  const yyyy = d.getFullYear();
  const mm = String(d.getMonth() + 1).padStart(2, "0");
  const dd = String(d.getDate()).padStart(2, "0");
  return `${yyyy}-${mm}-${dd}`;
}

function toISOLocalDateTime(value) {
  if (!value) return "";
  const d = new Date(value);
  if (Number.isNaN(d.getTime())) return String(value);
  const yyyy = d.getFullYear();
  const mm = String(d.getMonth() + 1).padStart(2, "0");
  const dd = String(d.getDate()).padStart(2, "0");
  const hh = String(d.getHours()).padStart(2, "0");
  const mi = String(d.getMinutes()).padStart(2, "0");
  return `${yyyy}-${mm}-${dd}T${hh}:${mi}`;
}

async function apiFetch(url, options = {}) {
  const res = await fetch(url, options);
  if (!res.ok) {
    const msg = await res.text().catch(() => "");
    throw new Error(msg || `HTTP ${res.status}`);
  }
  const text = await res.text().catch(() => "");
  if (!text) return null;
  try {
    return JSON.parse(text);
  } catch {
    return text;
  }
}

function getActiveModuleId() {
  const active = document.querySelector(".module-content.active");
  return active ? active.id : null;
}

function findTbodyByPossibleIds(possibleModuleIds) {
  for (const mid of possibleModuleIds) {
    const tbody = document.querySelector(`#${CSS.escape(mid)} table tbody`);
    if (tbody) return tbody;
  }
  return null;
}

// Loaders
async function loadCustomers() {
  const entity = ENTITIES.customers;
  const tbody = findTbodyByPossibleIds(entity.moduleIds);
  if (!tbody) throw new Error("Không tìm thấy <tbody> của module khách hàng (id customers/khachhang/khach-hang)");
  const data = await apiFetch(`${API_BASE}${entity.apiPrefix}/get-all`);
  tbody.innerHTML = "";
  (data || []).forEach((c) => {
    const tr = document.createElement("tr");
    tr.innerHTML = `
      <td>${c.khachHangID ?? ""}</td>
      <td>${c.tenKH ?? ""}</td>
      <td>${c.email ?? ""}</td>
      <td>${c.sdt ?? ""}</td>
      <td>${c.diaChi ?? ""}</td>
      <td>
        <button class="btn-edit" data-entity="customers" data-id="${c.khachHangID}">Sửa</button>
        <button class="btn-delete" data-entity="customers" data-id="${c.khachHangID}">Xóa</button>
      </td>
    `;
    tbody.appendChild(tr);
  });
}

async function loadContracts() {
  const entity = ENTITIES.contracts;
  const tbody = findTbodyByPossibleIds(entity.moduleIds);
  if (!tbody) throw new Error("Không tìm thấy <tbody> của module hợp đồng (id contracts/hopdong/hop-dong)");

  const data = await apiFetch(`${API_BASE}${entity.apiPrefix}/get-all`);
  tbody.innerHTML = "";

  (data || []).forEach((x) => {
    const tr = document.createElement("tr");
    tr.innerHTML = `
      <td>${x.hopDongID ?? ""}</td>
      <td>${x.khachHangID ?? ""}</td>
      <td>${toISODate(x.ngayKy)}</td>
      <td>${x.loaiDichVu ?? x.LoaiDichVu ?? ""}</td>
      <td>${x.giaTri ?? ""}</td>
      <td>${x.trangThai ?? ""}</td>
      <td>
        <button class="btn-edit" data-entity="contracts" data-id="${x.hopDongID}">Sửa</button>
        <button class="btn-delete" data-entity="contracts" data-id="${x.hopDongID}">Xóa</button>
      </td>
    `;
    tbody.appendChild(tr);
  });
}


async function loadCosts() {
  const entity = ENTITIES.costs;
  const tbody = findTbodyByPossibleIds(entity.moduleIds);
  if (!tbody) throw new Error("Không tìm thấy <tbody> của module chi phí (id costs/chiphi/chi-phi)");
  const data = await apiFetch(`${API_BASE}${entity.apiPrefix}/get-all`);
  tbody.innerHTML = "";
  (data || []).forEach((x) => {
    const tr = document.createElement("tr");
    tr.innerHTML = `
      <td>${x.chiPhiID ?? ""}</td>
      <td>${x.hopDongID ?? ""}</td>
      <td>${x.loaiChiPhi ?? ""}</td>
      <td>${x.soTien ?? ""}</td>
      <td>
        <button class="btn-edit" data-entity="costs" data-id="${x.chiPhiID}">Sửa</button>
        <button class="btn-delete" data-entity="costs" data-id="${x.chiPhiID}">Xóa</button>
      </td>
    `;
    tbody.appendChild(tr);
  });
}

async function loadInvoices() {
  const entity = ENTITIES.invoices;
  const tbody = findTbodyByPossibleIds(entity.moduleIds);
  if (!tbody) throw new Error("Không tìm thấy <tbody> của module hóa đơn (id invoices/hoadon/hoa-don)");
  const data = await apiFetch(`${API_BASE}${entity.apiPrefix}/get-all`);
  tbody.innerHTML = "";
  (data || []).forEach((x) => {
    const tr = document.createElement("tr");
    tr.innerHTML = `
      <td>${x.hoaDonID ?? ""}</td>
      <td>${x.hopDongID ?? ""}</td>
      <td>${x.soTien ?? ""}</td>
      <td>${toISODate(x.ngayLap)}</td>
      <td>${x.phanTramDaThanhToan ?? 0}</td>
      <td>
        <button class="btn-edit" data-entity="invoices" data-id="${x.hoaDonID}">Sửa</button>
        <button class="btn-delete" data-entity="invoices" data-id="${x.hoaDonID}">Xóa</button>
      </td>
    `;
    tbody.appendChild(tr);
  });
}

async function loadPayments() {
  const entity = ENTITIES.payments;
  const tbody = findTbodyByPossibleIds(entity.moduleIds);
  if (!tbody) throw new Error("Không tìm thấy <tbody> của module thanh toán (id payments/thanhtoan/thanh-toan)");
  const data = await apiFetch(`${API_BASE}${entity.apiPrefix}/get-all`);
  tbody.innerHTML = "";
  (data || []).forEach((x) => {
    const tr = document.createElement("tr");
    tr.innerHTML = `
      <td>${x.thanhToanID ?? ""}</td>
      <td>${x.hoaDonID ?? ""}</td>
      <td>${x.soTien ?? ""}</td>
      <td>${x.phuongThuc ?? ""}</td>
      <td>${toISOLocalDateTime(x.thoiGian)}</td>
      <td>
        <button class="btn-edit" data-entity="payments" data-id="${x.thanhToanID}">Sửa</button>
        <button class="btn-delete" data-entity="payments" data-id="${x.thanhToanID}">Xóa</button>
      </td>
    `;
    tbody.appendChild(tr);
  });
}

const LOADERS = {
  customers: loadCustomers,
  contracts: loadContracts,
  costs: loadCosts,
  invoices: loadInvoices,
  payments: loadPayments,
};

async function loadByModuleId(moduleId) {
  const key = findEntityKeyByModuleId(moduleId);
  if (!key) return;
  try {
    await LOADERS[key]();
  } catch (err) {
    console.error(`load ${key} error:`, err);
    alert(`Không load được dữ liệu ${ENTITIES[key].label}: ${err.message}`);
  }
}

// Dynamic Modal CRUD (uses #dynamicModal)
const FORM_SCHEMAS = {
  customers: [
    { name: "tenKH", label: "Tên khách hàng", type: "text", required: true, placeholder: "Nhập tên khách hàng..." },
    { name: "email", label: "Email", type: "email", required: false, placeholder: "example@gmail.com" },
    { name: "sdt", label: "SĐT", type: "text", required: false, placeholder: "0xxxxxxxxx" },
    { name: "diaChi", label: "Địa chỉ", type: "text", required: false, placeholder: "Nhập địa chỉ..." },
  ],
  contracts: [
    { name: "khachHangID", label: "Khách hàng (ID)", type: "number", required: true, placeholder: "Ví dụ: 1", parse: (v) => Number(v) },
    { name: "ngayKy", label: "Ngày ký", type: "date", required: true, normalize: toISODate, parse: (v) => v },
    { name: "loaiDichVu", label: "Loại dịch vụ", type: "text", required: false, placeholder: "Nhập loại dịch vụ...", parse: (v) => v },
    { name: "giaTri", label: "Giá trị", type: "number", required: true, placeholder: "0", parse: (v) => Number(v || 0) },
    { name: "trangThai", label: "Trạng thái", type: "text", required: false, placeholder: "Đang hiệu lực / Hết hạn...", parse: (v) => v },
  ],
  costs: [
    { name: "hopDongID", label: "Hợp đồng (ID)", type: "number", required: true, placeholder: "Ví dụ: 1", parse: (v) => Number(v) },
    { name: "containerID", label: "Container (ID)", type: "number", required: false, placeholder: "Có thể bỏ trống", parse: (v) => (v === "" ? null : Number(v)) },
    { name: "loaiChiPhi", label: "Loại chi phí", type: "text", required: false, placeholder: "Nhập loại chi phí...", parse: (v) => v },
    { name: "soTien", label: "Số tiền", type: "number", required: true, placeholder: "0", parse: (v) => Number(v || 0) },
    {
      name: "thuKhachHang",
      label: "Thu khách hàng",
      type: "select",
      required: true,
      options: ["Có", "Không"],
      defaultValue: "Không",
      parse: (v) => v,
    },
  ],
  payments: [
    { name: "hopDongID", label: "Hợp đồng (ID)", type: "number", required: true, placeholder: "Ví dụ: 1", parse: (v) => Number(v) },
    { name: "soTien", label: "Số tiền", type: "number", required: true, placeholder: "0", parse: (v) => Number(v || 0) },
    { name: "ngayLap", label: "Ngày phát hành", type: "date", required: true, normalize: toISODate, defaultValue: toISODate(new Date()), parse: (v) => v },
    { name: "phanTramDaThanhToan", label: "Đã thanh toán (%)", type: "number", required: true, placeholder: "0", defaultValue: 0, parse: (v) => Number(v || 0) },
  ],
  invoices: [
    { name: "hoaDonID", label: "Hóa đơn (ID)", type: "number", required: true, placeholder: "Ví dụ: 1", parse: (v) => Number(v) },
    { name: "soTien", label: "Số tiền", type: "number", required: true, placeholder: "0", parse: (v) => Number(v || 0) },
    { name: "phuongThuc", label: "Phương thức thanh toán", type: "text", required: false, placeholder: "Tiền mặt / Chuyển khoản...", parse: (v) => v },
    {
      name: "thoiGian",
      label: "Thời gian",
      type: "datetime-local",
      required: true,
      normalize: toISOLocalDateTime,
      defaultValue: toISOLocalDateTime(new Date()),
      parse: (v) => v,
    },
  ],
};

const DYNAMIC_MODAL = {
  modal: null,
  titleEl: null,
  form: null,
  entityIdInput: null,
  fieldsContainer: null,
  closeBtn: null,
  entityKey: null,
  mode: null, // create | edit
  currentData: {},
};

function initDynamicModal() {
  const modal = document.getElementById("dynamicModal");
  if (!modal) {
    console.warn("Không tìm thấy #dynamicModal. Chức năng Thêm/Sửa bằng modal sẽ không hoạt động.");
    return;
  }

  DYNAMIC_MODAL.modal = modal;
  DYNAMIC_MODAL.titleEl = document.getElementById("modalTitle") || modal.querySelector("#modalTitle");
  DYNAMIC_MODAL.form = document.getElementById("dynamicForm") || modal.querySelector("form");
  DYNAMIC_MODAL.entityIdInput = document.getElementById("entityId") || modal.querySelector("#entityId");
  DYNAMIC_MODAL.fieldsContainer = document.getElementById("formFields") || modal.querySelector("#formFields");
  DYNAMIC_MODAL.closeBtn = modal.querySelector(".close");

  if (DYNAMIC_MODAL.closeBtn) {
    DYNAMIC_MODAL.closeBtn.addEventListener("click", closeDynamicModal);
  }
  modal.addEventListener("click", (e) => {
    if (e.target === modal) closeDynamicModal();
  });

  if (DYNAMIC_MODAL.form) {
    DYNAMIC_MODAL.form.addEventListener("submit", submitDynamicForm);
  }
}

function openDynamicModal(entityKey, mode, data = {}, id = null) {
  if (!DYNAMIC_MODAL.modal) {
    alert("Chưa có modal #dynamicModal trong HTML.");
    return;
  }

  DYNAMIC_MODAL.entityKey = entityKey;
  DYNAMIC_MODAL.mode = mode;
  DYNAMIC_MODAL.currentData = data || {};

  if (DYNAMIC_MODAL.entityIdInput) {
    DYNAMIC_MODAL.entityIdInput.value = id ?? "";
  }

  const title = mode === "edit" ? `Sửa ${ENTITIES[entityKey].label}` : `Thêm ${ENTITIES[entityKey].label}`;
  if (DYNAMIC_MODAL.titleEl) DYNAMIC_MODAL.titleEl.textContent = title;

  renderDynamicFields(entityKey, data, mode);

  DYNAMIC_MODAL.modal.style.display = "block";
}

function closeDynamicModal() {
  if (!DYNAMIC_MODAL.modal) return;
  DYNAMIC_MODAL.modal.style.display = "none";
  if (DYNAMIC_MODAL.form) DYNAMIC_MODAL.form.reset();
  if (DYNAMIC_MODAL.fieldsContainer) DYNAMIC_MODAL.fieldsContainer.innerHTML = "";
  DYNAMIC_MODAL.entityKey = null;
  DYNAMIC_MODAL.mode = null;
  DYNAMIC_MODAL.currentData = {};
  if (DYNAMIC_MODAL.entityIdInput) DYNAMIC_MODAL.entityIdInput.value = "";
}

function renderDynamicFields(entityKey, data = {}, mode = "create") {
  const container = DYNAMIC_MODAL.fieldsContainer;
  if (!container) return;

  const schema = FORM_SCHEMAS[entityKey] || [];
  container.innerHTML = "";

  schema.forEach((f) => {
    const wrap = document.createElement("div");
    wrap.className = "form-group";

    const label = document.createElement("label");
    label.setAttribute("for", `f_${f.name}`);
    label.textContent = f.label + (f.required ? " *" : "");
    wrap.appendChild(label);

    let input;
    if (f.type === "select") {
      input = document.createElement("select");
      (f.options || []).forEach((opt) => {
        const o = document.createElement("option");
        o.value = opt;
        o.textContent = opt;
        input.appendChild(o);
      });
    } else {
      input = document.createElement("input");
      input.type = f.type || "text";
    }

    input.id = `f_${f.name}`;
    input.name = f.name;
    if (f.placeholder) input.placeholder = f.placeholder;
    if (f.required) input.required = true;

    // set value
    const raw = data?.[f.name];
    let v = raw;

    if (v == null || v === "") {
      if (mode === "create") {
        v = typeof f.defaultValue === "function" ? f.defaultValue() : f.defaultValue;
      }
    }

    if (f.normalize && v) v = f.normalize(v);

    if (v == null) v = "";
    input.value = String(v);

    wrap.appendChild(input);
    container.appendChild(wrap);
  });
}

function readDynamicFormBody(entityKey) {
  const schema = FORM_SCHEMAS[entityKey] || [];
  const body = { ...(DYNAMIC_MODAL.currentData || {}) };

  for (const f of schema) {
    const el = document.getElementById(`f_${f.name}`);
    if (!el) continue;

    const value = (el.value ?? "").trim();

    if (f.required && value === "") {
      el.focus();
      throw new Error(`Vui lòng nhập: ${f.label}`);
    }

    if (typeof f.parse === "function") {
      body[f.name] = f.parse(value);
    } else {
      body[f.name] = value;
    }
  }

  return body;
}

async function submitDynamicForm(ev) {
  ev.preventDefault();

  const entityKey = DYNAMIC_MODAL.entityKey;
  const mode = DYNAMIC_MODAL.mode;
  if (!entityKey || !mode) return;

  const e = ENTITIES[entityKey];
  const id = DYNAMIC_MODAL.entityIdInput ? DYNAMIC_MODAL.entityIdInput.value : "";

  try {
    const body = readDynamicFormBody(entityKey);

    if (mode === "create") {
      await apiFetch(`${API_BASE}${e.apiPrefix}/create`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body),
      });
      alert("Thêm thành công");
    } else {
      if (!id) throw new Error("Thiếu ID để sửa.");
      body[e.idField] = Number(id);
      await apiFetch(`${API_BASE}${e.apiPrefix}/update`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body),
      });
      alert("Sửa thành công");
    }

    closeDynamicModal();
    await LOADERS[entityKey]();
  } catch (err) {
    console.error("submit modal error:", err);
    alert((mode === "create" ? "Thêm" : "Sửa") + " thất bại: " + err.message);
  }
}

function openCreateModal(entityKey) {
  openDynamicModal(entityKey, "create", {}, null);
}

async function openEditModal(entityKey, id) {
  const e = ENTITIES[entityKey];
  const current = await apiFetch(`${API_BASE}${e.apiPrefix}/get-by-id/${id}`);
  openDynamicModal(entityKey, "edit", current || {}, id);
}

async function deleteRecord(entityKey, id) {
  const e = ENTITIES[entityKey];
  await apiFetch(`${API_BASE}${e.apiPrefix}/delete/${id}`, { method: "DELETE" });
  alert("Xóa thành công");
  await LOADERS[entityKey]();
}

// Global event handlers
// 1) Handle delete/edit/add buttons (event delegation)
document.addEventListener("click", async (ev) => {
  const delBtn = ev.target.closest(".btn-delete");
  if (delBtn) {
    const entityKey = delBtn.getAttribute("data-entity") || findEntityKeyByModuleId(getActiveModuleId());
    const id = delBtn.getAttribute("data-id");
    if (entityKey && id && confirm("Bạn chắc chắn muốn xóa?")) {
      try {
        await deleteRecord(entityKey, id);
      } catch (err) {
        console.error("delete error:", err);
        alert("Xóa thất bại: " + err.message);
      }
    }
    return;
  }

  const editBtn = ev.target.closest(".btn-edit");
  if (editBtn) {
    const entityKey = editBtn.getAttribute("data-entity") || findEntityKeyByModuleId(getActiveModuleId());
    const id = editBtn.getAttribute("data-id");
    if (entityKey && id) {
      try {
        await openEditModal(entityKey, id);
      } catch (err) {
        console.error("edit error:", err);
        alert("Sửa thất bại: " + err.message);
      }
    }
    return;
  }

  // "Thêm ..." button: bắt theo class btn-add hoặc text bắt đầu bằng "Thêm"
  const btn = ev.target.closest("button");
  if (btn) {
    const txt = (btn.textContent || "").trim().toLowerCase();
    const isAdd =
      btn.classList.contains("btn-add") ||
      txt.startsWith("thêm") ||
      txt.startsWith("them");

    if (isAdd) {
      const activeId = getActiveModuleId();
      const entityKey = findEntityKeyByModuleId(activeId);
      if (!entityKey) return;
      openCreateModal(entityKey);
      return;
    }
  }

  const a = ev.target.closest("a");
  if (!a) return;
  const onclickText = a.getAttribute("onclick") || "";
  const m = onclickText.match(/showModule\(\s*['"]([^'"]+)['"]\s*\)/i);
  if (m && m[1]) {
    const targetId = m[1];
    // đợi showModule chạy xong rồi mới load
    setTimeout(() => loadByModuleId(targetId), 0);
  }
});

document.addEventListener("DOMContentLoaded", () => {
  initDynamicModal();
  const activeId = getActiveModuleId();
  if (activeId) loadByModuleId(activeId);
});
