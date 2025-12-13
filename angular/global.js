// Thêm vào global.js
function showNotification(message, type = "success") {
    // ... (giữ nguyên như lần trước)
    const noti = document.createElement("div");
    noti.className = `notification ${type}`;
    noti.textContent = message;
    noti.style.cssText = `
        position: fixed; top: 20px; right: 20px; z-index: 10000;
        padding: 16px 24px; border-radius: 8px; color: white; font-weight: 500;
        background: ${type === "error" ? "#e74c3c" : "#27ae60"};
        box-shadow: 0 4px 12px rgba(0,0,0,0.15);
        animation: slideIn 0.4s;
    `;
    document.body.appendChild(noti);
    setTimeout(() => noti.remove(), 3500);
}