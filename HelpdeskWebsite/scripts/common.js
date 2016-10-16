// common - shared routines

// Ajax Call - returns a promise
function ajaxCall(type, url, data) {

    return $.ajax({
        type: type,
        url: url,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: true
    });
}

// common error
function errorRoutine(jqXHR) {

    if (jqXHR.responseJSON == null) {
        $("#LabelStatus").text(jqXHR.responseText);
    }
    else {
        $("#LabelStatus").text(jqXHR.responseJSON.Message);
    }
}