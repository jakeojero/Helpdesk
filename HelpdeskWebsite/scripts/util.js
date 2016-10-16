$(function () {
    buildTable();
}); // jQuery Default Function

// Main Div click
$("#main").click(function (e) { // click on any row

    var utilId = e.target.parentNode.id;

    if (utilId === "main" || utilId === "") {
        utilId = e.target.id; // ignore click
    }

    if (utilId === "0") {
        loadcollections();
    }
    else {
        //other utility options can go here
    }

}); //main click

//Build Initial Table
function buildTable() {

    $("#main").empty();

    div = $("<div class=\"list-group-item\" id=\"0\">" +
            "<span class=\"col-xs-10 h4\">Available Utilities</span>" +
            "</div>"
            );
    btn = $("<button class=\"list-group-item\" id=\"0\">" +
            "<span class=\"text-primary\">Re-Load Helpdesk Collections</span>" +
            "</button>"
        );

    btn.appendTo(div);
    div.appendTo($("#main"));

}// build table

function loadcollections() {

    $("#LabelStatus").text("Deleteing and Redefining Collections...");
    ajaxCall('Get', 'api/collections')
    .done(function (data) {
        $("#LabelStatus").text(data);
    })
    .fail(function (jqXHR, textStatus, errorThrown) {

        errorRoutine(jqXHR);

    });// ajax call

}// load collections