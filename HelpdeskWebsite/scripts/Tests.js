QUnit.test("Case Study Tests", function (assert) {
    assert.async(5);
    // Get All Employees
    ajaxCall("Get", "api/employees", "").then(function (data) {
        var numOfEmployees = data.length;
        assert.ok(numOfEmployees > 0, numOfEmployees + " Employees Retrieved");
        console.log("1");
    });

    // Get Smarty Pants
    ajaxCall("Get", "api/employeename/Smartypants", "").then(function (returnedemp) {
        assert.ok(returnedemp.Firstname == "Bigshot", "Employee Smartypants Retrieved");
        return ajaxCall("Get", "api/employees/" + returnedemp.Id, "");
        console.log("2");
    }).then(function (empById) {
        assert.ok(empById.Firstname == "Bigshot", "Employee " + empById.Lastname + " retrieved by Id " + empById.Id);
        console.log("3");
    })


    ajaxCall("Get", "api/employeename/Smartypants", "")
    .then(function (returnedemp) {
        var emp = new Object();
        emp.Title = "Mr.";
        emp.Firstname = "Jake";
        emp.Lastname = "Ojero";
        emp.Phoneno = "905-745-1850";
        emp.Email = "jakeojero@hotmail.com";
        emp.DepartmentId = returnedemp.DepartmentId;
        emp.Version = 1;
        return ajaxCall("Post", "api/employees", emp);
        console.log("4");
    }).then(function (empAdded) {
        assert.ok(empAdded[0] === "O", "Employee was Added");
        return ajaxCall("Get", "api/employeename/Ojero", "");
    }).then(function (returnedemp2) {
        assert.ok(returnedemp2.Firstname === "Jake", "New Employee " + returnedemp2.Lastname + " just added was retrieved, for delete");
        return ajaxCall("Delete", "api/employees/" + returnedemp2.Id);

    }).then(function (deleted) {
        assert.ok(deleted[0] === "O", "Employee Ojero was Deleted");
    });

    var emp = {};
    // Get Smartypants, then update him
    ajaxCall("Get", "api/employeename/Smartypants", "")
    .then(function (smarty) {
        emp = smarty;
        emp.Phoneno = "(555)555-5544";
        return ajaxCall("Put", "api/employees", emp);
    })
    .then(function (updatemsg) {
        assert.equal(updatemsg.indexOf("not"), -1, "First update for Employee Smartypants completed");
        emp.Phoneno = "444-444-4444";
        return ajaxCall("Put", "api/employees", emp);

    }).then(function (updatemsg2) {
        assert.ok(updatemsg2.indexOf("Stale") !== -1, "Second Update for Employee Smartypants was Stale");
    });
});