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
            $("#CloseModal").click(function () {
                $("#DepartmentModalForm").validate().resetForm();
            });
        }
        else {
            $("#ButtonDelete").hide();
            $("#ButtonAction").prop("value", "Add");
            $("#TextBoxDepartment").val("");
            localStorage.setItem("Id", "new");
        }

    });

    $("#ButtonDelete").click(function () {
        var deleteEmp = confirm("Really delete this department?");

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
            "<span class=\"col-xs-8 h4\">Department Name</span>" +
            "</div>"
        );

    div.appendTo($("#main"));
    employees = data;
    btn = $("<button class=\"list-group-item\" id=\"0\" " +
            "data-toggle=\"modal\" data-target=\"#myModal\">" +
            "<span class=\"text-primary glyphicon glyphicon-plus-sign\"></span><span class=\"text-primary\"> Add New Department...</span>"
        );
    btn.appendTo(div);

    $.each(data, function (index, dep) {
        var depId = dep.Id;
        btn = $("<button class=\"list-group-item\" id=\"" + depId +
                "\" data-toggle=\"modal\" data-target=\"#myModal\">");

        btn.html(
            "<span class=\"col-xs-4\" id=\"departmenttitle" + depId + "\">" + dep.Name + "</span>"
        );
        btn.appendTo(div);
    });// each

}//build table

function getAll(msg) {
    $("#LabelStatus").text("Departments Loading");

    ajaxCall("Get", "api/departments", "")
    .done(function (data) {
        buildTable(data);
        if (msg == "")
            $("#LabelStatus").text("Departments Loaded");
        else
            $("#LabelStatus").text(msg + " - Departments Loaded");
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $("#modal-body2").text("Retrieve Failed - Contact Tech Support");
        $("#modal-warning").modal("show");
        errorRoutine(jqXHR);
    });
}

function getById(id) {

    ajaxCall("Get", "api/departments/" + id, "").done(function (data) {
        if (data.Id != "not found") {
            copyInfoToModal(data);
        }
        else {
            $("#modalstatus").text("Failed to Load that Employee!");
            $("#modalstatus").addClass("alert alert-danger");
        }

    });
}

function copyInfoToModal(dep) {
    $("#TextBoxDepartment").val(dep.Name);
    localStorage.setItem("Id", dep.Id);
    localStorage.setItem("Version", dep.Version);
}

function validateDepartment() {
    $("#DepartmentModalForm").validate({
        rules: {
            Department: {
                required: true
            }

        },
        ignore: ".ignore, :hidden",
        errorElement: "div",
        wrapper: "div",
        messages: {
            Department: {
                required: "This field is required."
            }
        }
    });
}

function update() {
    validateDepartment();
    if ($("#DepartmentModalForm").valid()) {
        dep = new Object();
        dep.Name = $("#TextBoxDepartment").val();
        dep.Id = localStorage.getItem("Id");
        dep.Version = localStorage.getItem("Version");

        ajaxCall("Put", "api/departments", dep).done(function (data) {
            getAll(data);
            if (data[0] === "O") {
                $("#heading").css("background-color", "#47A44B");
                $("#heading").css("color", "#fff");
            }
            else if (data[0] === "D") {
                $("#heading").css("background-color", "#F0AD4E");
                $("#heading").css("color", "#fff");
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
   
    dep = new Object();
    dep.Id = localStorage.getItem("Id");

    ajaxCall("Delete", "api/departments/" + dep.Id).done(function (data) {
        getAll(data);
        if (data[0] === "O") {
            $("#heading").css("background-color", "#47A44B");
            $("#heading").css("color", "#fff");
        }
        else if (data[0] === "E") {
            $("#heading").css("background-color", "#F0AD4E");
            $("#heading").css("color", "#fff");
        }

    })
       .fail(function (jqXhr, textStatus, errorThrown) {
           errorRoutine(jqXhr);
       });
    $("#myModal").modal("hide");

    return false;
}

function create() {

    validateDepartment();

    if ($("#DepartmentModalForm").valid()) {
        dep = new Object();
        dep.Name = $("#TextBoxDepartment").val();
        dep.Version = 1;

        ajaxCall("Post", "api/departments", dep).done(function (data) {
            getAll(data);
            if (data[0] === "O") {
                $("#heading").css("background-color", "#47A44B");
                $("#heading").css("color", "#fff");
                $("#myModal").modal("hide");
            }
            else if (data[0] === "E") {
                $("#heading").css("background-color", "#F0AD4E");
                $("#heading").css("color", "#fff");
                $("#myModal").modal("hide");
            }
        })
        .fail(function (jqXhr, textStatus, errorThrown) {
            errorRoutine(jqXhr);
        });
    }

    return false;

}