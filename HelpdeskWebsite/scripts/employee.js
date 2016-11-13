$(function () {
    getAll("");

    $("#main").click(function (e) {

        var empId = e.target.parentNode.id;
        if (empId === "main" || empId === "") {
            empId = e.target.id;
        }

        if (empId !== "0") {
            $("#ButtonAction").prop("value", "Update");
            $("#ButtonDelete").show();
            $("#modalstatus").text("");
            $("#modalstatus").removeClass("alert alert-danger");
            getById(empId);
            $("#CloseModal").click(function () {
                $("#EmployeeModalForm").validate().resetForm();
            });
        }
        else {
            $("#StaffPicture").attr('src', '');
            $("#empname").text("");
            $("#CheckBoxTech").checked = false;
            $("#ButtonDelete").hide();
            $("#ButtonAction").prop("value", "Add");
            localStorage.setItem("Id", "new");
            $("#TextBoxTitle").val("");
            $("#TextBoxFirstname").val("");
            $("#TextBoxLastname").val("");
            $("#TextBoxPhone").val("");
            $("#TextBoxEmail").val("");
            loadDepartmentDDL(-1);
        }

    });

    $("#ButtonDelete").click(function () {
        var deleteEmp = confirm("Really delete this employee?");

        if (deleteEmp) {
            _delete();
            return !deleteEmp;
        }
        else
            return deleteEmp;
    });

    $("#ButtonAction").click(function () {

        if ($("#ButtonAction").val() === "Update") {
                update();   
        }
        else {
            create();
        }

        return false; // make sure to return false or REST calls get canceled

    });

}); // jquery default function

$("#main").click(function (e) {
    
    var empId = e.target.parentNode.id;
    if (empId === "main" || empId === "") {
        empId = e.target.id; // clicked on row somewhere else
    }

});


function buildTable(data) {
    $("#main").empty();

    div = $("<div class=\"list-group up-20\"><div>" +
            "<span class=\"col-xs-4 h4\">Title</span>" +
            "<span class=\"col-xs-4 h4\">First</span>" +
            "<span class=\"col-xs-4 h4\">Last</span>" +
            "</div>"
        );

    div.appendTo($("#main"));
    employees = data;
    btn = $("<button class=\"list-group-item\" id=\"0\" " +
            "data-toggle=\"modal\" data-target=\"#myModal\">" +
            "<span class=\"text-primary glyphicon glyphicon-plus-sign\"></span><span class=\"text-primary\"> Add New Employee...</span>"
        );
    btn.appendTo(div);

    $.each(data, function (index, emp) {
        var empId = emp.Id;
        btn = $("<button class=\"list-group-item\" id=\"" + empId +
                "\" data-toggle=\"modal\" data-target=\"#myModal\">");

        btn.html(
            "<span class=\"col-xs-4\" id=\"employeetitle" + empId + "\">" + emp.Title + "</span>" + 
            "<span class=\"col-xs-4\" id=\"employeefname" + empId + "\">" + emp.Firstname + "</span>" + 
            "<span class=\"col-xs-4\" id=\"emplastname" + empId + "\">" + emp.Lastname + "</span>"
        );
        btn.appendTo(div);
    });// each
    
}//build table

function getAll(msg) {
    $("#LabelStatus").text("Employees Loading");

    ajaxCall("Get", "api/employees", "")
    .done(function (data) {
        buildTable(data);
        if (msg == "")
            $("#LabelStatus").text("Employees Loaded");
        else
            $("#LabelStatus").text(msg + " - Employees Loaded");
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $("#modal-body2").text("Retrieve Failed - Contact Tech Support");
        $("#modal-warning").modal("show");
        errorRoutine(jqXHR);
    });
}

function getById(id) {

    ajaxCall("Get", "api/employees/" + id, "").done(function (data) {
        if (data.Id != "not found") {
            copyInfoToModal(data);
            localStorage.setItem("staffpic", data.StaffPicture64);
            $('#ImageHolder').html('<img class="profile-img-card" id="StaffPicture" height="120" width="110" src="data:image/png;base64,' + data.StaffPicture64 + '" />');
        }
        else {
            $("#modalstatus").text("Failed to Load that Problem!");
            $("#modalstatus").addClass("alert alert-danger");
        }

    });
}

function copyInfoToModal(emp) {
    $("#TextBoxTitle").val(emp.Title);
    $("#TextBoxFirstname").val(emp.Firstname);
    $("#TextBoxLastname").val(emp.Lastname);
    $("#TextBoxPhone").val(emp.Phoneno);
    $("#TextBoxEmail").val(emp.Email);
    $("#empname").text(emp.Firstname + ' ' + emp.Lastname);
    if (emp.IsTech)
        document.getElementById("CheckBoxTech").checked = true;
    else
        document.getElementById("CheckBoxTech").checked = false;

    localStorage.setItem("Id", emp.Id);
    localStorage.setItem("Version", emp.Version);
    loadDepartmentDDL(emp.DepartmentId);
}

function loadDepartmentDDL(empdep) {
    $.ajax({
        type: "Get",
        url: "api/departments",
        contentType: "application/json; charset=utf-8"
    })
    .done(function (data) {
        html = "";
        $("#ddlDepts").empty();
        $.each(data, function () {
            html += "<option value=\"" + this["Id"] + "\">" + this["Name"] + "</option>";
        });

        $("#ddlDepts").append(html);
        $("#ddlDepts").val(empdep);
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        alert("Error!");
    });

}

function validateEmployee() {
    $('#EmployeeModalForm').validate({ // initialize the plugin
        rules: {
            Title: {
                required: true,
                maxlength: 4,
                validTitle: true
            },
            Firstname: {
                required: true,
                maxlength: 25
            },
            Lastname: {
                required: true,
                maxlength: 25
            },
            Phone: {
                required: true,
                phoneUS: true,
                maxlength: 15
            },
            Email: {
                required: true,
                email: true,
                maxlength: 40
            },
            ddlDepts: {
                required: true
            }
        },
        ignore: ".ignore, :hidden",
        errorElement: "div",
        wrapper: "div",
        messages: {
            Title: {
                required: "Required 1-4 Characters.", maxlength: "Required 1-4 Characters", validTitle: "Mr. Ms. Mrs. or Dr."
            },
            Firstname: {
                required: "Required 1-25 Characters.", maxlength: "Required 1-25 Characters"
            },
            Lastname: {
                required: "Required 1-25 Characters.", maxlength: "Required 1-25 Characters"
            },
            Phone: {
                required: "Required 1-15 Characters.", maxlength: "Required 1-15 Characters"
            },
            Email: {
                required: "Required 1-40 Characters.", maxlength: "Required 1-40 Characters", email: "Must be a valid email format."
            }
        }
    });

    $.validator.addMethod("validTitle", function (value, element) {
        return this.optional(element) || (value === "Mr." || value === "Ms." || value === "Mrs." || value === "Dr.");
    }, "");
}

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#ImageHolder').html('<img class="profile-img-card" id="StaffPicture" height="120" width="110" src="' + e.target.result + '" />');
        };

        reader.readAsDataURL(input.files[0]);
    }
}

function update() {

    validateEmployee();
    emp = new Object();

    var reader = new FileReader();
    var file = $('#fileUpload')[0].files[0];
    if (file) {
        reader.readAsBinaryString(file);
        $("#modalstatus").text("");

        reader.onload = function (readerEvt) {

            var binaryString = reader.result;
            var encodedString = btoa(binaryString);
            emp.StaffPicture64 = encodedString;
            performUpdate();
        }
    }
    else {
        emp.StaffPicture64 = localStorage.getItem("staffpic");
        performUpdate();
    }
    

}

function performUpdate() {

    if ($("#EmployeeModalForm").valid()) {


        emp.Title = $("#TextBoxTitle").val();
        emp.Email = $("#TextBoxEmail").val();
        emp.Firstname = $("#TextBoxFirstname").val();
        emp.Lastname = $("#TextBoxLastname").val();
        emp.Phoneno = $("#TextBoxPhone").val();
        var checked = false;
        if ($("#CheckBoxTech").is(":checked")) {
            checked = true;
        }
        emp.IsTech = checked;
        emp.Id = localStorage.getItem("Id");
        emp.DepartmentId = $("#ddlDepts").val();
        emp.Version = localStorage.getItem("Version");

        ajaxCall("Put", "api/employees", emp).done(function (data) {
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
    emp = new Object();
    emp.Id = localStorage.getItem("Id");

    ajaxCall("Delete", "api/employees/" + emp.Id).done(function (data) {
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

    validateEmployee();
    emp = new Object();

    var reader = new FileReader();
    var file = $('#fileUpload')[0].files[0];
    if (file) {
        reader.readAsBinaryString(file);

        reader.onload = function (readerEvt) {
            var binaryString = reader.result;
            var encodedString = btoa(binaryString);
            emp.StaffPicture64 = encodedString;
            performCreate();
        }
    }
    else {
        $("#modalstatus").addClass("alert alert-danger");
        $("#modalstatus").text("You Must provide an employee picture");
    }

  

}

function performCreate() {
    if ($("#EmployeeModalForm").valid()) {

        emp.Title = $("#TextBoxTitle").val();
        emp.Email = $("#TextBoxEmail").val();
        emp.Firstname = $("#TextBoxFirstname").val();
        emp.Lastname = $("#TextBoxLastname").val();
        emp.Phoneno = $("#TextBoxPhone").val();
        emp.DepartmentId = $("#ddlDepts").val();
        var checked = false;
        if ($("#CheckBoxTech").is(":checked")) {
            checked = true;
        }
        emp.IsTech = checked;
        emp.Version = 1;

        ajaxCall("Post", "api/employees", emp).done(function (data) {
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