// Load the edit form via AJAX and display inside a Bootstrap modal

document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('a[data-popup="edit"]').forEach(function (link) {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            var url = link.getAttribute('href') + '?handler=Form';
            fetch(url, { headers: { 'X-Requested-With': 'XMLHttpRequest' } })
                .then(function (r) { return r.text(); })
                .then(function (html) {
                    var modalEl = document.getElementById('editModal');
                    modalEl.querySelector('.modal-content').innerHTML = html;
                    var modal = new bootstrap.Modal(modalEl);
                    modal.show();
                });
        });
    });
});
