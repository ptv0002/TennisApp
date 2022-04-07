// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    // ---------------- Script for tables using DataTable plug-in ----------------
    $('#normalTable').DataTable({
        order: [],
        language: {
            url: '//cdn.datatables.net/plug-ins/1.11.5/i18n/vi.json'
        },
        pageLength: 25
    });
    $('#noPaging').DataTable({
        order: [],
        language: {
            url: '//cdn.datatables.net/plug-ins/1.11.5/i18n/vi.json'
        },
        paging: false,
        searching: false,
        fixedHeader:true
    });

    // ---------------- Script for confirm modal popup ----------------
    // Show confirm modal popup
    var placeholderElement = $('#modal-placeholder');
    // Listen to the button and get data that's passed in
    $('button[data-toggle="ajax-modal"]').click(function (e) {
        e.preventDefault();
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        })
    })




})