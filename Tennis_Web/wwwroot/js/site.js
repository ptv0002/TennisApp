// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    // ---------------- Script for tables using DataTable plug-in ----------------
    $('#normalTable').DataTable({
        order: [],
        language: {
            url: '/dataTables/vi.json'
        },
        //initComplete: function () {
        //    //// Apply the search
        //    ////this.api().columns().every( function () {     // If you want to only show certain columns you can use a array, see below : 
        //    //this.api().columns([0]).every(function () {
        //    //    var that = this;

        //    $('#normalTable tfoot tr').appendTo('#normalTable thead');   // To displays the search boxs at the top instead to the bottom of the table
        //    //    $('input', this.footer()).on('keyup change clear', function () {
        //    //        if (that.search() !== this.value) {
        //    //            that
        //    //                .search(this.value)
        //    //                .draw();
        //    //        }
        //    //    });
        //    //});


        //    // -------------- here we add dropdown selectors filters to specified columns  --------------
        //    this.api().columns([1]).every(function () {
        //        var column = this;
        //        var select = $('<select class="form-control"><option  value="">Tất cả</option></select>')
        //            .appendTo($(column.footer()).empty())
        //            .on('change', function () {
        //                var val = $.fn.dataTable.util.escapeRegex(
        //                    $(this).val()
        //                );

        //                column
        //                    .search(val ? '^' + val + '$' : '', true, false)
        //                    .draw();
        //            });
        //    $(select).click(function (e) {
        //        e.stopPropagation();
        //    });
        //        column.data().unique().sort().each(function (d, j) {
        //            select.append('<option value="' + d + '">' + d + '</option>');
        //        });
        //    });


        //    //// -------------- here we add dropdown selectors specific for preview files  --------------
        //    //this.api().columns([2]).every(function () {
        //    //    var column = this;
        //    //    var select = $('<select class="form-control"><option  value=""></option></select>')
        //    //        .appendTo($(column.footer()).empty())
        //    //        .on('change', function () {
        //    //            var val = $.fn.dataTable.util.escapeRegex(
        //    //                $(this).val()
        //    //            );

        //    //            column
        //    //                .search(this.value)
        //    //                .draw();
        //    //        });

        //    //    {
        //    //        select.append('<option value="jpg">jpg</option>');
        //    //        select.append('<option value="png">png</option>');
        //    //        select.append('<option value="gif">gif</option>');
        //    //        select.append('<option value="mp4">mp4</option>');
        //    //    }
        //    //});
        //    //this.api().columns($('#filter')).every(function (d) {

        //    //    //This is used for specific column
        //    //    var column = this;
        //    //    var select = $(
        //    //        '<select class="form-control my-1"><option value="">Tất cả</option></select>'
        //    //    ).appendTo($(column.header()))
        //    //        .on('change', function () {
        //    //            var val = $.fn.dataTable.util.escapeRegex(
        //    //                $(this).val()
        //    //            );

        //    //            column
        //    //                .search(val ? '^' + val + '$' : '', true, false)
        //    //                .draw();
        //    //        });
        //    //    $(select).click(function (e) {
        //    //        e.stopPropagation();
        //    //    });
        //    //    column.data().unique().sort().each(function (d, j) {
        //    //        select.append('<option value="' + d + '">' + d + '</option>')
        //    //    });
        //    //});
           
        //}
    });
    var table = $('#noPaging').DataTable({
        'ajax': 'https://gyrocode.github.io/files/jquery-datatables/arrays_id.json',
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true,
                    'selectCallback': function (nodes, selected) {
                        // If "Show all" is not selected
                        if ($('#ctrl-show-selected').val() !== 'all') {
                            // Redraw table to include/exclude selected row
                            table.draw(false);
                        }
                    }
                },
            }
        ],
        'select': 'multi',
        /*'order': [[1, 'asc']],*/
        order: [],
        paging: false,
        language: {
            url: '/dataTables/vi.json'
        }
    });

    // Handle change event for "Show selected records" control
    $('#ctrl-show-selected').on('change', function () {
        var val = $(this).val();

        // If all records should be displayed
        if (val === 'all') {
            $.fn.dataTable.ext.search.pop();
            table.draw();
        }

        // If selected records should be displayed
        if (val === 'selected') {
            $.fn.dataTable.ext.search.pop();
            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex) {
                    return ($(table.row(dataIndex).node()).hasClass('selected')) ? true : false;
                }
            );

            table.draw();
        }

        // If selected records should not be displayed
        if (val === 'not-selected') {
            $.fn.dataTable.ext.search.pop();
            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex) {
                    return ($(table.row(dataIndex).node()).hasClass('selected')) ? false : true;
                }
            );

            table.draw();
        }
    });
    //$('#noPaging').DataTable({
    //    order: [],
    //    paging: false,
    //    language: {
    //        url: '/dataTables/vi.json'
    //    },
    //    initComplete: function () {
    //        $('#noPaging tfoot tr').appendTo('#noPaging thead');   // To displays the search boxs at the top instead to the bottom of the table
    //        this.api().columns([1]).every(function () {
    //            var column = this;
    //            var select = $('<input type="checkbox" id="chk" class="ml-3">')
    //                .appendTo($(column.footer()).empty())
    //                .on('change', function () {
    //                    var ischecked = $(this).is(':checked');
    //                    if (ischecked) {
    //                        // If selected records should be displayed
    //                        $.fn.dataTable.ext.search.pop();
    //                        $.fn.dataTable.ext.search.push(
    //                            function (settings, data, dataIndex) {
    //                                var row = table.row(dataIndex).node();
    //                                var checked = $('#chk_' + dataIndex).prop('checked');
    //                                var currentCheckChecked = $(row).find('input').prop('checked');
    //                                if (currentCheckChecked) {
    //                                    return true;
    //                                }

    //                                return false;
    //                            }
    //                        );

    //                        table.draw();

    //                    } else {
    //                        $.fn.dataTable.ext.search.pop();
    //                        $.fn.dataTable.ext.search.push(
    //                            function (settings, data, dataIndex) {
    //                                var row = table.row(dataIndex).node();
    //                                var checked = $('#chk_' + dataIndex).prop('checked');
    //                                var currentCheckChecked = $(row).find('input').prop('checked');
    //                                if (currentCheckChecked) {
    //                                    return true;
    //                                }

    //                                return true;
    //                            }
    //                        );

    //                        table.draw();
    //                    }
    //                });


    //        });
    //    }
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