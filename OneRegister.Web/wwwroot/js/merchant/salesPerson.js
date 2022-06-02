if (typeof (SalesPerson) === 'undefined') {
    var SalesPerson = {
        config: {
            gridId: 'salesPersonListGrid',
            salesPersonForm: 'salesPersonForm'
        },
        dataTable: {
            grid: {},
            init: function () {
                SalesPerson.dataTable.grid = $('#' + SalesPerson.config.gridId).DataTable({
                    ajax: {
                        url: "GetSalesPeople",
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
                            render: SalesPerson.dataTable.actionRender
                        },
                        { data: "id", visible: false },
                        { data: "name" },
                        { data: "email" },
                        { data: "assignedNumber" }
                    ]
                });
            },
            actionRender: function (data, type, row, meta) {
                var actions = `<a class="btn" title="Remove Person" onclick="SalesPerson.method.removePerson('${row.id}')"><i class="fa fa-trash text-danger"></i></a>`;
                return actions;
            }
        },
        method: {
            addPerson: function () {
                var isValid = app.validation.validateForm(SalesPerson.config.salesPersonForm);
                if (isValid) {
                    var formData = new FormData(document.getElementById(SalesPerson.config.salesPersonForm));
                    app.ajax.formData(formData, 'AddSalesPerson', SalesPerson.method.addPersonAjaxSuccess, true);
                }
            },
            addPersonAjaxSuccess: function (data) {
                if (data.isSuccessful) {
                    app.alert.showSuccessAutoHide('Sales Person is added successfully');
                    app.form.clearFormFields(SalesPerson.config.salesPersonForm);
                    SalesPerson.dataTable.grid.ajax.reload();
                } else {
                    app.validation.checkUnsuccessfulResponse(data);
                }
            },
            removePerson: function (id) {
                var formData = new FormData();
                formData.append('id', id);
                app.ajax.formData(formData, "RemoveSalesPerson", SalesPerson.method.removePersonAjaxSuccess, true);
            },
            removePersonAjaxSuccess: function (data) {
                if (data.isSuccessful) {
                    app.alert.showSuccessAutoHide('Role is removed successfully');
                    SalesPerson.dataTable.grid.ajax.reload();
                } else {
                    app.validation.checkUnsuccessfulResponse(data);
                }
            }
        },
        events: {
            addBtnClick: function () {
                $('#salesPersonAddBtn').on('click', function () {
                    SalesPerson.method.addPerson();
                });
            },
            registerAll: function () {
                app.event.tooltip();
                SalesPerson.dataTable.init();
                SalesPerson.events.addBtnClick();
            }
        }
    }
}

$(function () {
    SalesPerson.events.registerAll();
});