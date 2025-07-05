// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Generic confirmation handler for links and forms

document.addEventListener('click', function (e) {
    const target = e.target.closest('[data-confirm]');
    if (!target || target.tagName === 'FORM') return;

    const msg = target.getAttribute('data-confirm');
    if (!confirm(msg)) {
        e.preventDefault();
    }
});

document.addEventListener('submit', function (e) {
    const form = e.target.closest('form[data-confirm]');
    if (!form) return;

    const msg = form.getAttribute('data-confirm');
    if (!confirm(msg)) {
        e.preventDefault();
    }
});
