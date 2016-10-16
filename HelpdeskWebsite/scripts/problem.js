﻿$(function () {
    getAll("");

    $("#main").click(function (e) {

        var depId = e.target.parentNode.id;
        if (depId === "main" || depId === "") {
            depId = e.target.id;
        }

        if (depId !== "0") {
            $("#ButtonAction").prop("value", "Update");
            $("#ButtonDelete").show();
            getById(depId);
        }
        else {
            $("#ButtonDelete").hide();
            $("#ButtonAction").prop("value", "Add");
            localStorage.setItem("Id", "new");
        }

    });

    $("#ButtonDelete").click(function () {
        var deleteEmp = confirm("Really delete this problem?");

        if (deleteEmp) {
            _delete();
            return !deleteEmp;
        }
        else
            return deleteEmp;
    });

    $("#ButtonAction").click(function () {
        if ($("#ButtonAction").val() === "Update") {
            $("#modalstatus").text("Loading...");
            update();
            $("#modalstatus").text("");
        }
        else {
            create();
        }

        return false; // make sure to return false or REST calls get canceled

    });

}); // jquery default function

$("#main").click(function (e) {

    var depId = e.target.parentNode.id;
    if (depId === "main" || depId === "") {
        depId = e.target.id; // clicked on row somewhere else
    }
});


function buildTable(data) {
    $("#main").empty();

    div = $("<div class=\"list-group up-20\"><div>" +
            "<span class=\"col-xs-8 h4\">Problem Description</span>" +
            "</div>"
        );

    div.appendTo($("#main"));
    employees = data;
    btn = $("<button class=\"list-group-item\" id=\"0\" " +
            "data-toggle=\"modal\" data-target=\"#myModal\">" +
            "<span class=\"text-primary\">Add New Problem...</span>"
        );
    btn.appendTo(div);

    $.each(data, function (index, dep) {
        var depId = dep.Id;
        btn = $("<button class=\"list-group-item\" id=\"" + depId +
                "\" data-toggle=\"modal\" data-target=\"#myModal\">");

        btn.html(
            "<span class=\"col-xs-10\" id=\"problemtitle" + depId + "\">" + dep.Description + "</span>"
        );
        btn.appendTo(div);
    });// each

}//build table

function getAll(msg) {
    $("#LabelStatus").text("Problems Loading");

    ajaxCall("Get", "api/problems", "")
    .done(function (data) {
        buildTable(data);
        if (msg == "")
            $("#LabelStatus").text("Problems Loaded");
        else
            $("#LabelStatus").text(msg + " - Problems Loaded");
    }).fail(function (jqXHR, textStatus, errorThrown) {
        errorRoutine(jqXHR);
    });
}

function getById(id) {

    ajaxCall("Get", "api/problems/" + id, "").done(function (data) {
        if (data.Id != "not found") {
            copyInfoToModal(data);
        }
        else {
            $("#modalstatus").text("Failed to Load that Problem!");
            $("#modalstatus").addClass("alert alert-danger");
        }

    });
}

function copyInfoToModal(dep) {
    $("#TextBoxProblem").val(dep.Description);
    localStorage.setItem("Id", dep.Id);
    localStorage.setItem("Version", dep.Version);
}

function validateProblem() {
    $("#ProblemModalForm").validate({
        rules: {
            Problem: {
                required: true
            }

        }
    });
}

function update() {
    validateProblem();
    if ($("#ProblemModalForm").valid()) {
        prob = new Object();
        prob.Description = $("#TextBoxProblem").val();
        prob.Id = localStorage.getItem("Id");
        prob.Version = localStorage.getItem("Version");

        ajaxCall("Put", "api/problems", prob).done(function (data) {
            getAll(data);
            if (data[0] === "O") {
                $("#modal-body").text(data);
                $("#modal-success").modal("show");
            }
            else if (data[0] === "D") {
                $("#modal-body2").text(data);
                $("#modal-warning").modal("show");
            }

        })
        .fail(function (jqXhr, textStatus, errorThrown) {
            errorRoutine(jqXhr);
        });
        $("#myModal").modal("hide");
    }
    return false;
}

function _delete() {

    prob = new Object();
    prob.Id = localStorage.getItem("Id");

    ajaxCall("Delete", "api/problems/" + prob.Id).done(function (data) {
        getAll(data);
        if (data[0] === "O") {
            $("#modal-body").text(data);
            $("#modal-success").modal("show");
        }
        else if (data[0] === "E") {
            $("#modal-body2").text(data);
            $("#modal-warning").modal("show");
        }

    })
       .fail(function (jqXhr, textStatus, errorThrown) {
           errorRoutine(jqXhr);
       });
    $("#myModal").modal("hide");

    return false;
}

function create() {

    validateProblem();

    if ($("#ProblemModalForm").valid()) {
        prob = new Object();
        prob.Description = $("#TextBoxProblem").val();
        prob.Version = 1;
 
        ajaxCall("Post", "api/problems", prob).done(function (data) {
            getAll(data);
            if (data[0] === "O") {
                $("#modal-body").text(data);
                $("#modal-success").modal("show");
                $("#myModal").modal("hide");
            }
            else if (data[0] === "E") {
                $("#modal-body2").text(data);
                $("#modal-warning").modal("show");
                $("#myModal").modal("hide");
            }
        })
        .fail(function (jqXhr, textStatus, errorThrown) {
            errorRoutine(jqXhr);
        });
    }

    return false;

}