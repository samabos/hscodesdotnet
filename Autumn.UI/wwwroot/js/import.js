
$(document).ready(function () {
    // Sonstructs the suggestion engine
    var data = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.whitespace,
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        // The url points to a json file that contains an array of country names
        prefetch: 'ProductsJSON'
    });

    // Initializing the typeahead with remote dataset without highlighting
    $('.keyword').typeahead({
        classNames: {
            input: 'ui input',
            hint: 'Typeahead-hint big ui input',
            selectable: 'item',
            dataset: 'ui vertical menu'
        },
        hint: true,
        highlight: true,
        minLength: 1,
    },
        {
            name: 'data',
            source: data,
            limit: 50, /* Specify max number of suggestions to be displayed */

        }).ready(function (e) {
            console.log('here');
            $('.twitter-typeahead').removeClass('ui  action input');
            $('.twitter-typeahead').addClass('ui  action input').attr('Style', 'width:100%;');
        });

});

$("[data-toggle='modal']").click(function (e) {
    /* Prevent Default*/
    e.preventDefault();

    /* Parameters */
    var url = $(this).attr('href');
    var container = "#" + $(this).attr('data-container');
    var title = $(this).attr('data-title');

    /* XHR */
    $.get(url).done(function (data) {
        $(container + " .header").html(title);
        $(container + " .content").html(data);
        $(container).modal('show');
    });
});
$("#print").click(function () {
    //$("#NoteModal").print();
});

//$("[data-toggle='modal']").click(function (e) {
//    /* Prevent Default*/
//    e.preventDefault();

//    /* Parameters */
//    var url = $(this).attr('href');
//    var container = "#" + $(this).attr('data-container');
//    var title = $(this).attr('data-title');

//    /* XHR */
//    $.get(url).done(function (data) {
//        $(container + " .modal-title").html(title);
//        $(container + " .modal-body").html(data);
//        $(container).modal();
//    });
//    //.success(function () { $('input:text:visible:first').focus() });

//});
function SendHSCode(e) {
    var hscode = $(e).data("hscode");
    console.log(hscode);
    opener.CallParent(hscode);
}