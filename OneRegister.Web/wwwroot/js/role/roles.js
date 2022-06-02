"use strict"
if (typeof (Roles) === 'undefined') {
    var Roles = {
        config: {
            gridId: 'roleListGrid'
        },
        tree: {},
        dataTable: {
            grid: {},
            init: function () {
                Roles.dataTable.grid = $('#' + Roles.config.gridId).DataTable({
                    ajax: {
                        url: "GetRoles",
                        type: "POST",
                        dataSrc: ""
                    },
                    order: [[2, "asc"]],
                    columns: [
                        {
                            data: null,
                            defaultContent: "",
                            orderable: false,
                            searchable: false,
                            render: Roles.dataTable.actionRender
                        },
                        { data: "id", visible: false },
                        { data: "name" },
                        { data: "organization" },
                        {
                            data: "assignedNumber",
                            render: Roles.dataTable.assignedUserRender
                        }
                    ]
                });
            },
            actionRender: function (data, type, row, meta) {
                var actions = "";
                actions += `<a class="btn" title="Remove Role" onclick="Roles.method.removeRole('${row.id}')"><i class="fa fa-trash text-danger"></i></a>`;
                actions += `<a class="btn" title="Add Permission" onclick="Roles.method.changePermissions('${row.id}','${row.name}')"><i class="fas fa-user-shield text-primary"></i></a>`;
                return actions;
            },
            assignedUserRender: function (data, type, row, meta) {
                return `<a href="#" class="badge badge-secondary" title="Check Users" onclick="Roles.method.checkUsersInRole('${row.id}','${row.name}')">${data}</a>`;
            }
        },
        method: {
            addRole: function () {
                var isValid = app.validation.validateForm('roleForm');
                if (isValid) {
                    var formData = new FormData(document.getElementById('roleForm'));
                    app.ajax.formData(formData, 'AddRole', Roles.method.addRoleAjaxSuccess, true);
                }
            },
            addRoleAjaxSuccess: function (data) {
                if (data.isSuccessful) {
                    app.alert.showSuccessAutoHide('Role is added successfully');
                    Roles.dataTable.grid.ajax.reload();
                } else {
                    app.validation.checkUnsuccessfulResponse(data);
                }
            },
            removeRole: function (id) {
                var formData = new FormData();
                formData.append('roleId', id);
                app.ajax.formData(formData, "RemoveRole", Roles.method.removeRoleAjaxSuccess, true);
            },
            removeRoleAjaxSuccess: function (data) {
                if (data.isSuccessful) {
                    app.alert.showSuccessAutoHide('Role is removed successfully');
                    Roles.dataTable.grid.ajax.reload();
                } else {
                    app.validation.checkUnsuccessfulResponse(data);
                }
            },
            changePermissions: function (id, name) {
                var modal = app.confirmModal;
                modal.setParam(id);
                modal.setTitle(`Permission Management for ${name}`);
                modal.confirmAction = Roles.method.addPermissions;
                modal.setBody('<div id="permissionTree"></div>');
                Roles.method.initPermissionTree(id);
                modal.popUp();
            },
            addPermissions: function () {
                $.ajax({
                    url: 'UpdateRolePermissions',
                    type: 'POST',
                    dataType: 'JSON',
                    data: { 'roleId': app.confirmModal.paramId, 'permissions': Roles.tree.getCheckedNodes() },
                    success: function (data) {
                        if (data.isSuccessful) {
                            app.alert.showSuccessAutoHide("Permission(s) Added Successfully");
                        } else {
                            app.alert.showErrorAutoHide(data.message);
                        }
                    },
                    err: function (err) {
                        app.alert.showAjaxErrorAutoHide(err);
                    },
                    complete: function (xhr, status) {
                        app.confirmModal.dismiss();
                    }
                });
            },
            initPermissionTree: function (roleId) {
                Roles.tree = $("#permissionTree").tree({
                    primaryKey: 'id',
                    dataSource: {
                        url: 'GetPermissionTree',
                        data: { "roleId": roleId },
                        method: 'POST',
                        dataType: 'JSON'
                    },
                    uiLibrary: 'bootstrap4',
                    checkboxes: true
                });
            },
            checkUsersInRole: function (roleId,name) {
                app.confirmModal.setParam(roleId);
                app.confirmModal.setTitle(`Users belong to ${name}`);
                app.confirmModal.confirmAction = () => app.confirmModal.dismiss();
                $.ajax({
                    url: 'UsersInRole',
                    type: 'POST',
                    dataType: 'JSON',
                    data: { 'roleId': roleId },
                    success: function (data) {
                        var body = '<div><ul>';
                        data.forEach((x) => body += `<li><b>${x.userName}</b>-${x.name} (${x.isActive ? "Active" : "Inactive"})</li>`);
                        body += '</div></ul>'
                        app.confirmModal.setBody(body);
                        app.confirmModal.popUp();
                    },
                    err: function (err) {
                        app.confirmModal.dismiss();
                        app.alert.showAjaxErrorAutoHide(err);
                    }
                });
            }
        },
        events: {
            addBtnClick: function () {
                $('#roleAddBtn').on('click', function () {
                    Roles.method.addRole();
                });
            },
            registerAll: function () {
                app.event.tooltip();
                Roles.dataTable.init();
                Roles.events.addBtnClick();
            }
        }
    };
}

$(function () {
    Roles.events.registerAll();
});