$(function () {// Scroll to top button appear
    $('#normalTable').DataTable({
        order: [],
        language: {
            url: '/dataTables/vi.json'
        },
    });
    $('#noPaging').DataTable({
        order: [],
        language: {
            url: '/dataTables/vi.json'
        },
        paging: false,
        searching: false
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
    $(document).on('scroll', function () {
        var scrollDistance = $(this).scrollTop();
        if (scrollDistance > 100) {
            $('.scroll-to-top').fadeIn();
        } else {
            $('.scroll-to-top').fadeOut();
        }
    });

    // Smooth scrolling using jQuery easing
    $(document).on('click', 'a.scroll-to-top', function (e) {
        var $anchor = $(this);
        $('html, body').stop().animate({
            scrollTop: ($($anchor.attr('href')).offset().top)
        }, 1000, 'easeInOutExpo');
        e.preventDefault();
    });
})