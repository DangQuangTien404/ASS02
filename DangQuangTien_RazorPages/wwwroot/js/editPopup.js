// Load the edit form via AJAX and display inside a Bootstrap modal

document.addEventListener('click', function (e) {
    const link = e.target.closest('a[data-popup="edit"]');
    if (!link) return;

    e.preventDefault();
    const url = link.getAttribute('href') + '?handler=Form';

    fetch(url, { headers: { 'X-Requested-With': 'XMLHttpRequest' } })
        .then(function (r) { return r.text(); })
        .then(function (html) {
            const modalEl = document.getElementById('editModal');
            modalEl.querySelector('.modal-content').innerHTML = html;
            const modal = new bootstrap.Modal(modalEl);
            modal.show();
        });
});
