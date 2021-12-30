// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    // ---------------- Script for confirm modal popup ----------------
    // Show confirm modal popup
    var placeholderElement = $('#modal-placeholder');
    // Listen to the button and get data that's passed in
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        })
    })
    // Send data to the given url if the confirm button is clicked
    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();

        $.post(actionUrl, dataToSend).done(function (data) {
            placeholderElement.find('.modal').modal('hide');
        })
    })
    // ---------------- Script for updating TL_Bang - Parameter Tab ----------------
    function UpdateTL_Bang() {
        var tl_vd = $("#TL_VoDich").val();
        var tl_ck = $("#TL_ChungKet").val();
        var tl_bk = $("#TL_BanKet").val();
        var tl_tk = $("#TL_TuKet").val();
        var tl_bang = 100 - tl_vd - tl_ck - tl_bk - tl_tk
        $("#TL_Bang").val(tl_bang)
    }
    $('#TL_VoDich').on('input', function (event) {
        UpdateTL_Bang()
        event.preventDefault();
    });
    $('#TL_ChungKet').on('input', function (event) {
        event.preventDefault();
        UpdateTL_Bang()
    });
    $('#TL_BanKet').on('input', function (event) {
        event.preventDefault();
        UpdateTL_Bang()
    });
    $('#TL_TuKet').on('input', function (event) {
        event.preventDefault();
        UpdateTL_Bang()
    });
    $('#addButton').click(function () {
        $('<tr id="tablerow' + counter + '"><td>' +
            '<input class="text-box single-line" name="ClientName[' + counter + ']" type="text" value="" required="required" />' +
            '</td>' +
            '<td>' +
            '<input type="email" class="text-box single-line" name="Email[' + counter + ']" value="" required="required" />' +
            '</td>' +
            '<td>' +
            '<button type="button" class="btn btn-primary" onclick="removeTr(' + counter + ');">Delete</button>' +
            '</td>' +
            '</tr>').appendTo('#clientTable');
        counter++;
        return false;
    });
    $("body").on("click", "#btnAdd", function () {
        //Reference the Name and Country TextBoxes.
        var txtName = $("#txtName");
        var txtCountry = $("#txtCountry");

        //Get the reference of the Table's TBODY element.
        var tBody = $("#tblCustomers > TBODY")[0];

        //Add Row.
        var row = tBody.insertRow(-1);

        //Add Name cell.
        var cell = $(row.insertCell(-1));
        cell.html(txtName.val());

        //Add Country cell.
        cell = $(row.insertCell(-1));
        cell.html(txtCountry.val());

        //Add Button cell.
        cell = $(row.insertCell(-1));
        var btnRemove = $("<input />");
        btnRemove.attr("type", "button");
        btnRemove.attr("onclick", "Remove(this);");
        btnRemove.val("Remove");
        cell.append(btnRemove);

        //Clear the TextBoxes.
        txtName.val("");
        txtCountry.val("");
    });    
})