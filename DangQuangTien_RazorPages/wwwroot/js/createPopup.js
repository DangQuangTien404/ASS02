// Script to show create pages in an iframe inside a Bootstrap modal

document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('a[data-popup="create"]').forEach(function (link) {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            var url = link.getAttribute('href');
            var modalEl = document.getElementById('createModal');
            modalEl.querySelector('.modal-content').innerHTML = '<iframe src="' + url + '" class="w-100" style="height:80vh;border:0;"></iframe>';
            var modal = new bootstrap.Modal(modalEl);
            modal.show();
        });
    });
});
