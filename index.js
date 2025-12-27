function showModule(id) {
    document.querySelectorAll('.module-content').forEach(m => m.classList.remove('active'));
    document.getElementById(id).classList.add('active');
}
