// Load the create form via AJAX and display inside a Bootstrap modal

document.addEventListener('click', function (e) {
    const link = e.target.closest('a[data-popup="create"]');
    if (!link) return;

    e.preventDefault();
    const url = link.getAttribute('href') + '?handler=Form';

    fetch(url, { headers: { 'X-Requested-With': 'XMLHttpRequest' } })
        .then(function (r) { return r.text(); })
        .then(function (html) {
            const modalEl = document.getElementById('createModal');
            modalEl.querySelector('.modal-body').innerHTML = html;
            const modal = new bootstrap.Modal(modalEl);
            modal.show();
        });
});

// Handle submit inside the modal via AJAX
document.addEventListener('submit', function (e) {
    const form = e.target.closest('#createModal form');
    if (!form) return;

    e.preventDefault();
    const url = form.getAttribute('action');
    const formData = new FormData(form);

    fetch(url, {
        method: 'POST',
        body: formData,
        headers: { 'X-Requested-With': 'XMLHttpRequest' }
    }).then(function (r) {
        if (r.redirected) {
            window.location = r.url;
            return;
        }
        return r.text().then(function (html) {
            document.querySelector('#createModal .modal-body').innerHTML = html;
        });
    });
});
