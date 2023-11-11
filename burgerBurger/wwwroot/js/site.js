// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function showSchedule() {
    var cb = document.getElementById('scheduleCheck').checked;
    var form = document.getElementById('delivery');

    if (cb)
        form.className = 'form-group';
    else
        form.className = 'form-group d-none';
}
