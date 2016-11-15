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
    else if (utilId === "1") {
        //other utility options can go here
        $("#LabelStatus").text("Generating Employee Report");
        generateEmployeeReport();
    }
    else if (utilId === "2") {
        $("#LabelStatus").text("Generating Call Report");
        generateCallReport();
    }

}); //main click

//Build Initial Table
function buildTable() {

    $("#main").empty();

    div = $("<div class=\"list-group-item\" id=\"\">" +
            "<span class=\"col-xs-10 h4\">Available Utilities</span>" +
            "</div>"
            );
    btn = $("<button class=\"list-group-item\" id=\"0\">" +
            "<span class=\"text-primary\">Re-Load Helpdesk Collections</span>" +
            "</button>" +
            "<button class=\"list-group-item\" id=\"1\">" +
            "<span class=\"text-primary\">Generate Employee Report</span>" +
            "</button>" +
            "<button class=\"list-group-item\" id=\"2\">" +
            "<span class=\"text-primary\">Generate Call Report</span>" +
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

function generateEmployeeReport() {
    ajaxCall("Get", "api/employeereport", "").done(function (data) {
        $("#LabelStatus").text("Generating Employee Report!");
        if (data === "report generated") {
            window.open('/pdfs/EmployeeReport.pdf');
            return false;
        }
    }).fail(function (jqXhr, textStatus, errorThrown) {
        errorRoutine(jqXhr);
    });
}

function generateCallReport() {
    ajaxCall("Get", "api/callreport", "").done(function (data) {
        $("#LabelStatus").text("Generating Call Report!");
        if (data === "report generated") {
            window.open('/pdfs/CallReport.pdf');
            return false;
        }
    }).fail(function (jqXhr, textStatus, errorThrown) {
        errorRoutine(jqXhr);
    });
}

function ajaxCall(type, url, data) {
    return $.ajax({//return the promise that `$.ajax` returns
        type: type,
        url: url,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: true
    });
}

function errorRoutine(jqXHR) {//common error
    if (jqXHR.responseJson == null) {
        $("#lblstatus").text(jqXHR.responseText);
    }
    else {
        $("#lblstatus").text(jqXHR.responseJson.Message);
    }
}