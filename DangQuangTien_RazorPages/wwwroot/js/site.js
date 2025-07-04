// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Generic confirmation handler for links and forms
document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('[data-confirm]').forEach(function (el) {
        var msg = el.getAttribute('data-confirm');
        if (el.tagName === 'FORM') {
            el.addEventListener('submit', function (e) {
                if (!confirm(msg)) {
                    e.preventDefault();
                }
            });
        } else {
            el.addEventListener('click', function (e) {
                if (!confirm(msg)) {
                    e.preventDefault();
                }
            });
        }
    });
});
