var app = {
    properties: {
        billboard: {
            model: {
                type: 0,
                text: "",
                url: ""
            },
            type: {
                notFound: 0,
                success: 1,
                failure: 2
            }
        },
        validation: {
            isValid: false
        },
        BaseURL: ""
    },
    utils: {
        loadingModal: {},
        initLoading: function () {
            app.utils.loadingModal = $('#loadingDialog').modal({
                show: false,
                keyboard: false,
                backdrop: false
            });
        },
        hideLoading: function () {
            app.utils.loadingModal.modal('hide');
        },
        showLoading: function () {
            app.utils.loadingModal.modal('show');
        },
        refreshSelect: function (selectId, data) {
            var list = $('#' + selectId);
            list.empty();
            for (d of data) {
                list.append(new Option(d.text, d.value));
            }
        },
        showBillboard: function (formData) {
            var form = document.createElement('form');
            form.method = "POST";
            form.action = app.properties.BaseURL + "Billboard";
            for (var name in formData) {
                var input = document.createElement('input');
                input.type = 'hidden';
                input.name = name;
                input.value = formData[name];
                form.appendChild(input);
            }
            document.body.appendChild(form);
            form.submit();
        },
        setBaseUrl: function (url) {
            app.properties.BaseURL = url;
        },
        redirectTo: function (url) {
            window.location.replace(app.properties.BaseURL + url);
        }
    },
    alert: {
        goto: function (alertId) {
            $([document.documentElement, document.body]).animate({
                scrollTop: $('#' + alertId).offset().top
            }, 500);
        },
        hide: function () {
            $('#alert-box div').html('');
            $('#alert-box').addClass("d-none");
        },
        autoHide: function () {
            setTimeout(function () {
                app.alert.hide();
            }, 3000);
        },
        showInfo: function (text) {
            app.alert.show(text, 'alert-info');
        },
        showWarning: function (text) {
            app.alert.show(text, 'alert-warning');
        },
        showError: function (text) {
            app.alert.show(text, 'alert-danger');
        },
        showAjaxError: function (err) {
            var text = err.statusText;
            if (text.includes("Unauthorized") || text.includes("Ajax error")) {
                window.location = '/Account/Logout';
            };
            app.alert.error.show(text);
        },
        showSuccess: function (text) {
            app.alert.show(text, 'alert-success');
        },
        showInfoAutoHide: function (text) {
            app.alert.show(text, 'alert-info');
            app.alert.autoHide();
        },
        showWarningAutoHide: function (text) {
            app.alert.show(text, 'alert-warning');
            app.alert.autoHide();
        },
        showErrorAutoHide: function (text) {
            app.alert.show(text, 'alert-danger');
            app.alert.autoHide();
        },
        showAjaxErrorAutoHide: function (err) {
            app.alert.showAjaxError(err);
            app.alert.autoHide();
        },
        showSuccessAutoHide: function (text) {
            app.alert.show(text, 'alert-success');
            app.alert.autoHide();
        },
        makeHtmlFromFullResponse: function (data, showFields = true) {
            var alertContent = '';
            if (data.message) {
                alertContent += '<p>' + data.message + '</p>';
            }
            if (showFields) {
                if (data.validations.length > 0) {
                    alertContent += '<ul>';
                    for (var i = 0; i < data.validations.length; i++) {
                        alertContent += '<li>' + data.validations[i].field + ' ' + data.validations[i].description + '</li>';
                    }
                    alertContent += '</ul>'
                }
            } else {
                if (data.validations.length > 0) {
                    alertContent += '<ul>';
                    for (var i = 0; i < data.validations.length; i++) {
                        alertContent += '<li>' + data.validations[i].description + '</li>';
                    }
                    alertContent += '</ul>'
                }
            }

            return alertContent;
        },
        show: function (text, type) {
            app.loading.hide();
            $('#alert-box div').html(text);
            $('#alert-box').removeClass().addClass('alert alert-dismissible ' + type);
        }
    },
    confirmModal: {
        paramId: "",
        confirmAction: function () { },
        setTitle: function (title) {
            $('#confirmModal .modal-header h5').text(title);
        },
        setBody: function (text) {
            $('#confirmModal .modal-body').html(text);
        },
        clearBody: function () {
            $('#confirmModal .modal-body').html('');
        },
        setParam: function (id) {
            app.confirmModal.paramId = id;
        },
        popUp: function () {
            $('#confirmModal').modal('show');
        },
        dismiss: function () {
            $('#confirmModal').modal('hide');
        }
    },
    loading: {
        loadingModal: null,
        hide: function () {
            if (app.loading.loadingModal !== null) {
                app.loading.loadingModal.modal('hide');
            }
        },
        show: function () {
            if (app.loading.loadingModal === null) {
                app.loading.loadingModal = $('#loadingDialog').modal({
                    show: true,
                    keyboard: false,
                    backdrop: false
                });
            } else {
                app.loading.loadingModal.modal('show');
            }
        }
    },
    event: {
        replaceFileBtn: function (id) {
            $(`[data-file-for="${id}"]`).toggleClass('d-none');
        },
        sideMenu: {
            toggle: function () {
                $("#menu-toggle").click(function (e) {
                    e.preventDefault();
                    $("#wrapper").toggleClass("toggled");
                });
            },
            wrapper: function () {
                $("#sidebar-wrapper").click(function () {
                    if ($("#wrapper").hasClass("toggled") && window.matchMedia('(min-width: 768px)').matches) {
                        $("#wrapper").addClass("hoverToggled");
                        $("#wrapper #sidebar-wrapper a i:last-child, #wrapper #sidebar-wrapper .sidebar-heading i").removeClass("icon-fade");
                    };
                });
            },
            pageContentWrapper: function () {
                $("#page-content-wrapper").click(function () {
                    if ($("#wrapper").hasClass("hoverToggled") && window.matchMedia('(min-width: 768px)').matches) {
                        $('#wrapper').removeClass("hoverToggled");
                        $("#wrapper #sidebar-wrapper a i:last-child, #wrapper #sidebar-wrapper .sidebar-heading i").addClass("icon-fade");
                        $('.collapse').collapse('hide');
                    }
                });
            },
            register: function () {
                app.event.sideMenu.toggle();
                app.event.sideMenu.wrapper();
                app.event.sideMenu.pageContentWrapper();
            }
        },
        photoFrame: {
            click: function (imageFileId) {
                $('#picHolder').on('click', function () {
                    $('#' + imageFileId).trigger('click');
                });
            }
        },
        customFile: function () {
            $(".custom-file-input").on("change", function () {
                var fileName = $(this).val().split("\\").pop();
                $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
            });
        },
        select2: function () {
            $('.select2').select2({
                theme: 'bootstrap4'
            });
        },
        tooltip: function () {
            $(function () {
                $('[data-toggle="tooltip"]').tooltip()
            })
        },
        register: function () {
            var e = app.event;
            e.sideMenu.register();
        }
    },
    validation: {
        alert: {
            hide: function (id) {
                $(`[data-val-text = "${id}"]`).addClass("d-none");
            },
            show: function (id, text) {
                $(`[data-val-text = "${id}"]`).html(text).removeClass("d-none");
            },
            reset: function () {
                app.alert.hide();
                $('[data-val-text]').addClass("d-none");
            }
        },
        showFormErrors: function (validations) {
            for (var i = 0; i < validations.length; i++) {
                app.validation.alert.show(validations[i].field, validations[i].description);
            }
        },
        checkUnsuccessfulResponse: function (fullResponse) {
            app.validation.showFormErrors(fullResponse.validations);
            if (fullResponse.message) {
                app.alert.showError(fullResponse.message);
            }
        },
        validators: {
            required: function (inputId) {
                var value = app.validation.getInputVal(inputId);
                if (!value) {
                    app.properties.validation.isValid = false;
                    app.validation.alert.show(inputId, "Required");
                }
            }
        },
        validateForm: function (formId) {
            app.validation.alert.reset();
            app.properties.validation.isValid = true;
            var form = $('#' + formId);
            if (form[0]) {
                for (var i = 0; i < form[0].length; i++) {
                    if (form[0][i].dataset.validations) {
                        app.validation.validateInput(form[0][i].id, form[0][i].dataset.validations);
                    }
                }
            }
            return app.properties.validation.isValid;
        },
        validateInput: function (inputId, validationsStr) {
            var validations = validationsStr.split(',');
            validations.forEach(function (validationRule) {
                if (validationRule.includes('Required')) {
                    app.validation.validators.required(inputId);
                }
            });
        },
        getInputVal: function (inputId) {
            var input = $('#' + inputId);
            var type = input.prop('type');
            switch (type) {
                case 'date':
                case 'password':
                case 'email':
                case 'select-one':
                case 'text':
                case 'number':
                case 'textarea':
                    return $('#' + inputId).val();
                    break;
                case 'fieldset':
                    return $('#' + inputId + ' :checked').val();
                    break;
                case 'file':
                    //?? $('#' + inputId).val() unknown.jpg
                    var link = $('#' + inputId + 'Holder' + ' a').attr('href');
                    if (link !== undefined && link.includes('unknown.jpg')) return undefined;
                    return link;
                    break;
                default:
                    console.error(`The ${type} isn't yet support in ${inputId}`);
                    return undefined;
            }
        }
    },
    ajax: {
        formData: function (formData, url, success, autoHide = false) {
            $.ajax({
                url: url,
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                dataType: 'JSON',
                success: success,
                error: function (err) {
                    if (autoHide) {
                        app.alert.showAjaxErrorAutoHide(err);
                    } else {
                        app.alert.showAjaxError(err);
                    }
                }
            });
        },
        postSimple: function (url, data, autoHide = false) {
            $.ajax({
                url: url,
                type: 'POST',
                data: data,
                dataType: 'JSON',
                success: function (data) {
                    if (data.isSuccessful) {
                        if (data.message == null || data.message.length === 0) {
                            data.message = "It's done successfully!";
                        }
                        if (autoHide) {
                            app.alert.showSuccessAutoHide(data.message);
                        } else {
                            app.alert.showSuccess(data.message)
                        }
                    } else {
                        if (autoHide) {
                            app.alert.showErrorAutoHide(data.message);
                        } else {
                            app.alert.showAjaxError(data.message);
                        }
                    }
                },
                error: function (err) {
                    if (autoHide) {
                        app.alert.showAjaxErrorAutoHide(err);
                    } else {
                        app.alert.showAjaxError(err);
                    }
                }
            });
        },
        post: function (url, data, success, failure) {
            $.ajax({
                url: url,
                type: 'POST',
                data: data,
                dataType: 'JSON',
                beforeSend: function () {
                    app.loading.show();
                },
                success: function (data) {
                    success(data);
                },
                error: function (err) {
                    failure(err);
                },
                complete: function () {
                    app.loading.hide();
                }
            });
        }
    },
    form: {
        clearFormFields: function (formId) {
            var form = document.getElementById(formId);
            if (form) {
                form.reset();
            }
        },
        submit: function (formId, url, success) {
            var isValid = app.validation.validateForm(formId);
            if (isValid) {
                var fd = new FormData(document.getElementById(formId));
                $.ajax({
                    url: url,
                    type: 'POST',
                    data: fd,
                    processData: false,
                    contentType: false,
                    dataType: 'JSON',
                    beforeSend: function () {
                        app.loading.show();
                    },
                    success: function (data) {
                        if (data.isSuccessful) {
                            success(data);
                        } else {
                            app.validation.checkUnsuccessfulResponse(data);
                        }
                    },
                    error: function (err) {
                        app.alert.showAjaxError(err);
                    },
                    complete: function () {
                        app.loading.hide();
                    }
                });
            }
        },
        fillForm: function (formName, data) {
            $.each(data, function (key, value) {
                $('#' + formName).find("[name='" + key + "']").val(value);
            });
        }
    },
    modal: {
        show: function (htmlString) {
            $('#formModal .modal-dialog').html(htmlString);
            $('#formModal').modal('show');
        },
        hide: function () {
            $('#formModal .modal-dialog').html('');
            $('#formModal').modal('hide');
        }
    }
};

$(function () {
    app.event.register();
});