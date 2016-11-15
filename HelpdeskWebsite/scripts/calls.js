$(function () {
    getAll("");

    $("#main").click(function (e) {

        var callId = e.target.parentNode.id;
        if (callId === "main" || callId === "") {
            callId = e.target.id;
        }

        if (callId !== "0") {
            $("#ButtonAction").prop("value", "Update");
            $("#ButtonDelete").show();
            getById(callId);
            $("#CloseModal").click(function () {
                $("#CallModalForm").validate().resetForm();
            });
        }
        else {
            loadEmployeeDDL();
            loadProblemDDL();
            loadTechDDL();
            clearFormVisually();

            $("#LabelDateOpened").text(formatDate());
            $("#DateOpened").val(formatDate());
            $("#ButtonDelete").hide();
            $("#ButtonAction").prop("value", "Add");
            $("#TextBoxDepartment").val("");
            localStorage.setItem("Id", "new");
        }

    });

    $("#CheckBoxClose").click(function () {
        if ($("#CheckBoxClose").is(":checked")) {
            $("#LabelDateClosed").text(formatDate());
            $("#DateClosed").val(formatDate());
            localStorage.setItem("openstatus", false);
        }
        else {
            $("#LabelDateClosed").text("");
            $("#DateClosed").val("");
            localStorage.setItem("openstatus", true);
        }
    });

    $("#ButtonDelete").click(function () {
        var deleteEmp = confirm("Really delete this Call?");

        if (deleteEmp) {
            _delete();
            return !deleteEmp;
        }
        else
            return deleteEmp;
    });

    localStorage.setItem("openstatus", true); // For the Call Update
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

    var callId = e.target.parentNode.id;
    if (callId === "main" || callId === "") {
        callId = e.target.id; // clicked on row somewhere else
    }
});

function clearFormVisually() {
    $("#ButtonAction").show();
    $("#Notes").attr("readonly", false);
    $("#ddlTechs").prop('disabled', false);
    $("#ddlEmployees").prop('disabled', false);
    $("#ddlProblems").prop('disabled', false);
    $("#CheckBoxClose").prop('disabled', false);
    $("#CheckBoxClose").prop('checked', false);
    $("#LabelDateClosed").text("");
    $("#DateClosed").val("");
    $("#Notes").val("");
}

function buildTable(data) {
    
    $("#main").empty();

    div = $("<div class=\"list-group up-20\"><div>" +
            "<span class=\"col-xs-4 h4\">Date Opened</span>" +
            "<span class=\"col-xs-4 h4\">For</span>" +
            "<span class=\"col-xs-4 h4\">Problem</span>" +
            "</div>"
        );

    div.appendTo($("#main"));
    call = data;
    btn = $("<button class=\"list-group-item\" id=\"0\" " +
            "data-toggle=\"modal\" data-target=\"#myModal\">" +
            "<span class=\"text-primary glyphicon glyphicon-plus-sign\"></span><span class=\"text-primary\"> Add New Call...</span>"
        );
    btn.appendTo(div);

    $.each(data, function (index, call) {
        var callId = call.Id;
        btn = $("<button class=\"list-group-item\" id=\"" + callId +
                "\" data-toggle=\"modal\" data-target=\"#myModal\">");

        btn.html(
            "<span class=\"col-xs-4\" id=\"calldate" + callId + "\">" + formatDate(call.DateOpened) + "</span>" +
            "<span class=\"col-xs-4\" id=\"calldate" + callId + "\">" + call.EmployeeName + "</span>" +
            "<span class=\"col-xs-4\" id=\"calldate" + callId + "\">" + call.ProblemDescription + "</span>"

        );
        btn.appendTo(div);
    });// each

}//build table

function getAll(msg) {
    $("#LabelStatus").text("Calls Loading");

    ajaxCall("Get", "api/calls", "")
    .done(function (data) {
        buildTable(data);
        if (msg == "")
            $("#LabelStatus").text("Calls Loaded");
        else
            $("#LabelStatus").text(msg + " - Calls Loaded");
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $("#modal-body2").text("Retrieve Failed - Contact Tech Support");
        $("#modal-warning").modal("show");
        errorRoutine(jqXHR);
    });
}

function loadEmployeeDDL(empdep) {
    $.ajax({
        type: "Get",
        url: "api/employees",
        contentType: "application/json; charset=utf-8"
    })
    .done(function (data) {
        html = "";
        $("#ddlEmployees").empty();
        $.each(data, function () {
            html += "<option value=\"" + this["Id"] + "\">"  + " " + this["Firstname"] + ", " + this["Lastname"] + "</option>";
        });

        $("#ddlEmployees").append(html);
        $("#ddlEmployees").val(empdep);
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        alert("Error!");
    });

}
function loadProblemDDL(empdep) {
    $.ajax({
        type: "Get",
        url: "api/problems",
        contentType: "application/json; charset=utf-8"
    })
    .done(function (data) {
        html = "";
        $("#ddlProblems").empty();
        $.each(data, function () {
            html += "<option value=\"" + this["Id"] + "\">" + this["Description"] + "</option>";
        });

        $("#ddlProblems").append(html);
        $("#ddlProblems").val(empdep);
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        alert("Error!");
    });

}
function loadTechDDL(empdep)
 {
    $.ajax({
        type: "Get",
        url: "api/employees",
        contentType: "application/json; charset=utf-8"
    })
    .done(function (data) {
        html = "";
        $("#ddlTechs").empty();
        $.each(data, function () {
            if(this["IsTech"])
                html += "<option value=\"" + this["Id"] + "\">" + " " + this["Firstname"] + ", " + this["Lastname"] + "</option>";
        });

        $("#ddlTechs").append(html);
        $("#ddlTechs").val(empdep);
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        alert("Error!");
    });

}

function getById(id) {

    ajaxCall("Get", "api/calls/" + id, "").done(function (data) {
        if (data.Id != "not found") {
            copyInfoToModal(data);
        }
        else {
            $("#modalstatus").text("Failed to Load that Employee!");
            $("#modalstatus").addClass("alert alert-danger");
        }

    });
}

function copyInfoToModal(call) {
    loadEmployeeDDL(call.EmployeeId);
    loadProblemDDL(call.ProblemId);
    loadTechDDL(call.TechId);
    $("#LabelDateOpened").text(formatDate(call.DateOpened));
    $("#DateOpened").val(call.DateOpened);
    $("#Notes").val(call.Notes);
    if (!call.OpenStatus) {
        $("#LabelDateClosed").text(formatDate(call.DateClosed));
        $("#DateClosed").val(call.DateClosed);
        $("#ButtonAction").hide();
        $("#Notes").attr("readonly", "readonly");
        $("#ddlTechs").prop('disabled', true);
        $("#ddlEmployees").prop('disabled', true);
        $("#ddlProblems").prop('disabled', true);
        $("#CheckBoxClose").prop('checked', true);
        $("#CheckBoxClose").prop('disabled', true);
    }
    else {
        $("#LabelDateClosed").text("");
        $("#DateClosed").val("");
        $("#ButtonAction").show();
        $("#Notes").attr("readonly", false);
        $("#ddlTechs").prop('disabled', false);
        $("#ddlEmployees").prop('disabled', false);
        $("#ddlProblems").prop('disabled', false);
        $("#CheckBoxClose").prop('checked', false);
        $("#CheckBoxClose").prop('disabled', false);
    }
    localStorage.setItem("Id", call.Id);
    localStorage.setItem("Version", call.Version);
}

function validateDepartment() {
    $("#CallModalForm").validate({
        rules: {
            ddlEmployees: {
                required: true
            },
            ddlProblems: {
                required: true
            },
            ddlTechs: {
                required: true
            },
            Notes: {
                required: true,
                maxlength: 250
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
    if ($("#CallModalForm").valid()) {
        call = new Object();
        call.EmployeeId = $("#ddlEmployees").val();
        call.ProblemId = $("#ddlProblems").val();
        call.TechId = $("#ddlTechs").val();
        call.Notes = $("#Notes").val();
        call.DateOpened = $("#DateOpened").val();
        call.DateClosed = $("#DateClosed").val();
        call.OpenStatus = localStorage.getItem("openstatus");
        call.Id = localStorage.getItem("Id");
        call.Version = localStorage.getItem("Version");

        ajaxCall("Put", "api/calls", call).done(function (data) {
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
   
    call = new Object();
    call.Id = localStorage.getItem("Id");

    ajaxCall("Delete", "api/calls/" + call.Id).done(function (data) {
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

    if ($("#CallModalForm").valid()) {
        call = new Object();
        call.ProblemId = $("#ddlProblems").val();
        call.EmployeeId = $("#ddlEmployees").val();
        call.TechId = $("#ddlTechs").val();
        call.DateOpened = $("#DateOpened").val();
        call.DateClosed = null;
        call.OpenStatus = true;
        call.Notes = $("#Notes").val();
        call.Version = 1;

        ajaxCall("Post", "api/calls", call).done(function (data) {
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

function formatDate(date) {
    var d;
    if (date === undefined) {
        d = new Date(); // No Date coming from server
    }
    else {
        var d = new Date(Date.parse(date));
    }

    var _day = d.getDate();
    var _month = d.getMonth() + 1;
    var _year = d.getFullYear();
    var _hour = d.getHours();
    var _min = d.getMinutes();
    if (_min < 10) { _min = "0" + _min; }
    return _year + "-" + _month + "-" + _day + " " + _hour + ":" + _min;
}