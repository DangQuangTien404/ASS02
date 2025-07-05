// Load the delete confirmation via AJAX and display inside a Bootstrap modal

document.addEventListener('click', function (e) {
    const link = e.target.closest('a[data-popup="delete"]');
    if (!link) return;

    e.preventDefault();
    const url = link.getAttribute('href') + '?handler=Form';

    fetch(url, { headers: { 'X-Requested-With': 'XMLHttpRequest' } })
        .then(function (r) { return r.text(); })
        .then(function (html) {
            const modalEl = document.getElementById('deleteModal');
            modalEl.querySelector('.modal-body').innerHTML = html;
            const modal = new bootstrap.Modal(modalEl);
            modal.show();
        });
});

// Handle submit inside the modal via AJAX
document.addEventListener('submit', function (e) {
    const form = e.target.closest('#deleteModal form');
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

        const contentType = r.headers.get('Content-Type') || '';
        if (contentType.indexOf('application/json') !== -1) {
            r.json().then(function (data) {
                alert(data.error);
                const modal = bootstrap.Modal.getInstance(document.getElementById('deleteModal'));
                if (modal) modal.hide();
            });
            return;
        }

        r.text().then(function (html) {
            document.querySelector('#deleteModal .modal-body').innerHTML = html;
        });
    });
});
