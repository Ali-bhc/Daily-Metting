$(document).on('click', '.page-link', function (e) {
    e.preventDefault();
    var page = $(this).data('page');
    //Console.log("page");

    $.ajax({
        url: '/Admin/History',
        data: { page: page },
        type: 'GET',
        success: function (result) {
            $('.submissions').html(result);
        }
    });
});
