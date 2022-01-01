// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    // ---------------- Script for confirm modal popup ----------------
    // Show confirm modal popup
    var placeholderElement = $('#modal-placeholder');
    // Listen to the button and get data that's passed in
    $('button[data-toggle="ajax-modal"]').click(function () {
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

        $.post(actionUrl, dataToSend).done(function () {
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
        $("#TL_Bang").val(tl_bang);
    };
    $('#TL_VoDich').on('input', function () {
        UpdateTL_Bang();
    });
    $('#TL_ChungKet').on('input', function () {
        UpdateTL_Bang();
    });
    $('#TL_BanKet').on('input', function () {
        UpdateTL_Bang();
    });
    $('#TL_TuKet').on('input', function () {
        UpdateTL_Bang();
    });
    //$('#addLevel').on('click', function (event) {
    //    event.preventDefault();
    //    //Reference the Level TextBoxes.
    //    var level = $("#txtLevel");

    //    //Get the reference of the Table's TBODY element.
    //    var tBody = $("#tblLevel > TBODY")[0];

    //    //Add Row.
    //    var row = tBody.insertRow(-1);

    //    //Add Level cell.

    //    var lastRowId = tBody.children.length();
    //    var cell = $(row.insertCell(-1));
    //    var inputForm = '@Model.DS_Trinh.Add(new Models.DS_Trinh { Trinh = Convert.ToInt32(' + level.val() + ') });<input asp-for="DS_Trinh.ToList()[' + lastRowId + '].Trinh" value="' + level.val() + '" disabled />';
    //    var input = $(inputForm);
    //    cell.append(input);
        
    //    //Add empty cell.
    //    cell = $(row.insertCell(-1));

    //    //Clear the TextBoxes.
    //    level.val("");
    //});    
})