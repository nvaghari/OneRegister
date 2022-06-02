if (typeof (MerchantRegister) === "undefined") {
    var MerchantRegister = {
        properties: {
            mid: "Info_Mid",
            registerationState: {
                New: 0,
                Info: 1,
                Services: 2,
                Owners: 3,
                Outlets: 4,
                BankAccount: 5,
                Files: 6,
                Supplementary: 7,
                Complete: 8,
                Channel: 9,
                Terms: 10
            },
            collapses: {
                services: "collapseServices",
                info: "collapseInfo",
                owner: "collapseOwner",
                outlet: "collapseOutlet",
                bank: "collapseBank",
                file: "collapseFile",
                channel: "collapseChannel",
                terms: "collapseTerms"
            },
            alerts: {
                services: "servicesAlert",
                info: "infoAlert",
                owner: "ownerAlert",
                ownerMaster: "ownerMasterAlert",
                outlet: "outletAlert",
                outletMaster: "outletMasterAlert",
                bank: "bankAlert",
                file: "fileAlert",
                channel: "channelAlert",
                reject: "rejectAlert",
                terms: "termsAlert"
            },
            modals: {
                owner: "ownerModal",
                outlet: "outletModal",
                reject: "rejectModal"
            },
            forms: {
                services: "formServices",
                info: "formInfo",
                owner: "formOwner",
                outlet: "formOutlet",
                bank: "formBank",
                file: "formFile",
                supplementary: "formSupplementary",
                channel: "formChannel",
                reject: "formReject",
                terms: "formTerms"
            },
            grids: {
                owner: {
                    id: "ownerTable",
                    table: null
                },
                outlet: {
                    id: "outletTable",
                    table: null
                }
            }
        },
        method: {
            registerInfo: function () {
                MerchantRegister.util.alert.clear(MerchantRegister.properties.alerts.info);
                var isValid = app.validation.validateForm(MerchantRegister.properties.forms.info);
                if (isValid) {
                    var formData = new FormData(document.getElementById(MerchantRegister.properties.forms.info));
                    formData.append("Mid", MerchantRegister.privateMethod.getMerchantId());
                    $.ajax({
                        url: 'RegisterInfo',
                        type: 'POST',
                        data: formData,
                        processData: false,
                        contentType: false,
                        dataType: 'JSON',
                        success: MerchantRegister.privateMethod.ajaxInfoSuccess,
                        error: function (err) {
                            MerchantRegister.privateMethod.ajaxError(err, MerchantRegister.properties.alerts.info);
                        }
                    });
                }
            },
            registerServices: function () {
                MerchantRegister.util.alert.clear(MerchantRegister.properties.alerts.services);
                var formData = new FormData(document.getElementById(MerchantRegister.properties.forms.services));
                var selectedServices = 0;
                for (var item of formData.keys()) {
                    selectedServices++;
                }
                if (selectedServices === 0) {
                    MerchantRegister.util.alert.showText(MerchantRegister.properties.alerts.services, 'At least one service is required')
                    return;
                }
                var isValid = app.validation.validateForm(MerchantRegister.properties.forms.services);
                if (isValid) {
                    formData.append("Mid", MerchantRegister.privateMethod.getMerchantId());
                    $.ajax({
                        url: 'RegisterServices',
                        type: 'POST',
                        data: formData,
                        processData: false,
                        contentType: false,
                        dataType: 'JSON',
                        success: MerchantRegister.privateMethod.ajaxServicesSuccess,
                        error: function (err) {
                            MerchantRegister.privateMethod.ajaxError(err, MerchantRegister.properties.alerts.services);
                        }
                    });
                }
            },
            addOwner: function () {
                var isValid = app.validation.validateForm(MerchantRegister.properties.forms.owner);
                if (isValid) {
                    var formData = new FormData(document.getElementById(MerchantRegister.properties.forms.owner));
                    formData.append("Mid", MerchantRegister.privateMethod.getMerchantId());
                    $.ajax({
                        url: 'AddOwner',
                        type: 'POST',
                        data: formData,
                        processData: false,
                        contentType: false,
                        dataType: 'JSON',
                        success: MerchantRegister.privateMethod.ajaxAddOwnerSuccess,
                        error: function (err) {
                            MerchantRegister.privateMethod.ajaxError(err, MerchantRegister.properties.alerts.owner);
                        }
                    });
                }
            },
            getOwner: function (ownerId) {
                $.ajax({
                    url: 'GetOwner',
                    type: 'POST',
                    data: { "id": ownerId },
                    success: function (data) {
                        app.modal.show(data);
                    },
                    error: function (err) {
                        MerchantRegister.privateMethod.ajaxError(err, MerchantRegister.properties.alerts.ownerMaster);
                    }
                });
            },
            removeOwner: function (ownerId) {
                app.confirmModal.setTitle("Remove Company Owner/Directors");
                app.confirmModal.setBody("are you sure to remove this item?");
                app.confirmModal.paramId = ownerId;
                app.confirmModal.confirmAction = MerchantRegister.method.removeOwnerConfirm;
                app.confirmModal.popUp();
            },
            removeOwnerConfirm: function () {
                $.ajax({
                    url: 'RemoveOwner',
                    type: 'POST',
                    data: { "ownerId": app.confirmModal.paramId },
                    dataType: 'JSON',
                    success: MerchantRegister.privateMethod.ajaxRemoveOwnerSuccess,
                    error: function (err) {
                        MerchantRegister.privateMethod.ajaxError(err, MerchantRegister.properties.alerts.ownerMaster);
                    },
                    complete: function () {
                        app.confirmModal.dismiss();
                    }
                });
            },
            continueOwner: function () {
                if (MerchantRegister.properties.grids.owner.table.rows().count() == 0) {
                    MerchantRegister.util.alert.showText(MerchantRegister.properties.alerts.ownerMaster, "At least one Owner is required");
                } else {
                    var mid = MerchantRegister.privateMethod.getMerchantId();
                    MerchantRegister.method.changeMerchantState(mid, MerchantRegister.properties.registerationState.Owners, MerchantRegister.properties.alerts.ownerMaster);
                }
            },
            addOutlet: function () {
                var isValid = app.validation.validateForm(MerchantRegister.properties.forms.outlet);
                if (isValid) {
                    var formData = new FormData(document.getElementById(MerchantRegister.properties.forms.outlet));
                    formData.append("Mid", MerchantRegister.privateMethod.getMerchantId());
                    $.ajax({
                        url: 'AddOutlet',
                        type: 'POST',
                        data: formData,
                        processData: false,
                        contentType: false,
                        dataType: 'JSON',
                        success: MerchantRegister.privateMethod.ajaxAddOutletSuccess,
                        error: function (err) {
                            MerchantRegister.privateMethod.ajaxError(err, MerchantRegister.properties.alerts.outlet);
                        }
                    });
                }
            },
            getOutlet: function (outletId) {
                $.ajax({
                    url: 'GetOutlet',
                    type: 'POST',
                    data: { "id": outletId },
                    success: function (data) {
                        app.modal.show(data);
                    },
                    error: function (err) {
                        MerchantRegister.privateMethod.ajaxError(err, MerchantRegister.properties.alerts.outletMaster);
                    }
                });
            },
            removeOutlet: function (outletId) {
                app.confirmModal.setTitle("Remove Physical Channel - Main Outlet Detail");
                app.confirmModal.setBody("are you sure to remove this item?");
                app.confirmModal.paramId = outletId;
                app.confirmModal.confirmAction = MerchantRegister.method.removeOutletConfirm;
                app.confirmModal.popUp();
            },
            removeOutletConfirm: function () {
                $.ajax({
                    url: 'RemoveOutlet',
                    type: 'POST',
                    data: { "outletId": app.confirmModal.paramId },
                    dataType: 'JSON',
                    success: MerchantRegister.privateMethod.ajaxRemoveOutletSuccess,
                    error: function (err) {
                        MerchantRegister.privateMethod.ajaxError(err, MerchantRegister.properties.alerts.outletMaster);
                    },
                    complete: function () {
                        app.confirmModal.dismiss();
                    }
                });
            },
            continueOutlet: function () {
                if (MerchantRegister.properties.grids.outlet.table.rows().count() == 0) {
                    MerchantRegister.util.alert.showText(MerchantRegister.properties.alerts.outletMaster, "At least one Outlet or Physical Channel is required");
                } else {
                    var mid = MerchantRegister.privateMethod.getMerchantId();
                    MerchantRegister.method.changeMerchantState(mid, MerchantRegister.properties.registerationState.Outlets, MerchantRegister.properties.alerts.outletMaster);
                }
            },
            saveBankAccount: function () {
                MerchantRegister.util.alert.clear(MerchantRegister.properties.alerts.bank);
                var isValid = app.validation.validateForm(MerchantRegister.properties.forms.bank);
                if (isValid) {
                    var formData = new FormData(document.getElementById(MerchantRegister.properties.forms.bank));
                    formData.append("Mid", MerchantRegister.privateMethod.getMerchantId());
                    $.ajax({
                        url: 'AddBankAccount',
                        type: 'POST',
                        data: formData,
                        processData: false,
                        contentType: false,
                        dataType: 'JSON',
                        success: MerchantRegister.privateMethod.ajaxBankSuccess,
                        error: function (err) {
                            MerchantRegister.privateMethod.ajaxError(err, MerchantRegister.properties.alerts.channel);
                        }
                    });
                }
            },
            saveSupplementaryInfo: function () {
                console.log("not implemented yet");
            },
            uploadFile: function (id) {
                if (!MerchantRegister.validations.isFileValid(id)) return;
                var inputFile = $('#' + id).get(0);
                var files = inputFile.files;
                var formData = new FormData();
                formData.append("id", id);
                formData.append("file", files[0], files[0].name);
                formData.append("mid", MerchantRegister.privateMethod.getMerchantId());
                $.ajax({
                    url: 'UploadFile',
                    method: 'POST',
                    contentType: false,
                    processData: false,
                    data: formData,
                    success: function (data) {
                        MerchantRegister.privateMethod.ajaxUploadSuccess(id, data);
                    },
                    error: MerchantRegister.privateMethod.ajaxUploadError
                });
            },
            removeFile: function (id) {
                $.ajax({
                    url: 'RemoveFile',
                    method: 'POST',
                    data: { 'mid': MerchantRegister.privateMethod.getMerchantId(), 'name': id },
                    success: function (data) {
                        MerchantRegister.privateMethod.ajaxUploadSuccess(id, data);
                    },
                    error: MerchantRegister.privateMethod.ajaxUploadError
                });
            },
            continueFile: function () {
                var isValid = app.validation.validateForm(MerchantRegister.properties.forms.file);
                if (isValid) {
                    var mid = MerchantRegister.privateMethod.getMerchantId();
                    MerchantRegister.method.changeMerchantState(mid, MerchantRegister.properties.registerationState.Files, MerchantRegister.properties.alerts.file);
                }
            },
            continueTerms: function () {
                var isTermsAccepted = document.getElementById('tcCheck').checked;
                if (isTermsAccepted) {
                    var mid = MerchantRegister.privateMethod.getMerchantId();
                    MerchantRegister.method.changeMerchantState(mid, MerchantRegister.properties.registerationState.Terms, MerchantRegister.properties.alerts.terms);
                } else {
                    MerchantRegister.util.alert.showText(MerchantRegister.properties.alerts.terms, "Please read and accept our terms and conditions");
                }
            },
            changeMerchantState: function (merchantId, state, sectionId) {
                MerchantRegister.util.alert.hide(sectionId);
                $.ajax({
                    url: "ChangeMerchantState",
                    type: "POST",
                    data: { 'merchantId': merchantId, 'state': state },
                    success: function (data) {
                        if (data.isSuccessful) {
                            MerchantRegister.stateMachine.currenState = state;
                            MerchantRegister.stateMachine.checkState();
                        } else {
                            MerchantRegister.util.alert.showText(sectionId, data.message);
                        }
                    },
                    error: function (err) {
                        MerchantRegister.util.alert.showText(sectionId, err.statusText);
                    }
                });
            },
            complete: function () {
                if (document.getElementById('tcCheck').checked) {
                    var mid = MerchantRegister.privateMethod.getMerchantId();
                    $.ajax({
                        url: "Complete",
                        type: "POST",
                        data: { 'mid': mid },
                        success: function (data) {
                            if (data.isSuccessful) {
                                var model = app.properties.billboard.model;
                                model.type = app.properties.billboard.type.success;
                                model.text = "Merchant registration completed successfully.";
                                model.url = app.properties.BaseURL + "Merchant/List";
                                app.utils.showBillboard(model);
                            } else {
                                var err = app.alert.makeHtmlFromFullResponse(data, false);
                                app.alert.showError(err);
                            }
                        },
                        error: function (err) {
                            app.alert.showAjaxError(err);
                        }
                    });
                } else {
                    app.alert.showError('Please read and accept our terms and conditions');
                }
            },
            saveChannel: function () {
                MerchantRegister.util.alert.clear(MerchantRegister.properties.alerts.channel);
                var isValid = app.validation.validateForm(MerchantRegister.properties.forms.channel);
                if (isValid) {
                    var formData = new FormData(document.getElementById(MerchantRegister.properties.forms.channel));
                    formData.append("Mid", MerchantRegister.privateMethod.getMerchantId());
                    $.ajax({
                        url: 'AddChannel',
                        type: 'POST',
                        data: formData,
                        processData: false,
                        contentType: false,
                        dataType: 'JSON',
                        success: MerchantRegister.privateMethod.ajaxChannelSuccess,
                        error: function (err) {
                            MerchantRegister.privateMethod.ajaxError(err, MerchantRegister.properties.alerts.bank);
                        }
                    });
                }
            },
            approve: function () {
                $.ajax({
                    url: 'Approve',
                    type: 'POST',
                    data: { 'mid': app.confirmModal.paramId },
                    success: function (data) {
                        if (data.isSuccessful) {
                            window.location = app.properties.BaseURL + "Merchant/Inbox";
                        } else {
                            var err = app.alert.makeHtmlFromFullResponse(data);
                            app.alert.showError(err);
                        }
                    },
                    error: function (err) {
                        app.alert.showAjaxError(err);
                    },
                    complete: function () {
                        app.confirmModal.dismiss();
                    }
                });
            },
            accept: function () {
                $.ajax({
                    url: 'Accept',
                    type: 'POST',
                    data: { 'mid': app.confirmModal.paramId },
                    success: function (data) {
                        if (data.isSuccessful) {
                            window.location = app.properties.BaseURL + "Merchant/Inbox";
                        } else {
                            var err = app.alert.makeHtmlFromFullResponse(data);
                            app.alert.showError(err);
                        }
                    },
                    error: function (err) {
                        app.alert.showAjaxError(err);
                    },
                    complete: function () {
                        app.confirmModal.dismiss();
                    }
                });
            },
            reject: function () {
                var isValid = app.validation.validateForm(MerchantRegister.properties.forms.reject);
                if (isValid) {
                    var formData = new FormData(document.getElementById(MerchantRegister.properties.forms.reject));
                    formData.append("Mid", MerchantRegister.privateMethod.getMerchantId());
                    $.ajax({
                        url: 'Reject',
                        type: 'POST',
                        data: formData,
                        processData: false,
                        contentType: false,
                        dataType: 'JSON',
                        success: MerchantRegister.privateMethod.ajaxRejectSuccess,
                        error: function (err) {
                            MerchantRegister.privateMethod.ajaxError(err, MerchantRegister.properties.alerts.reject);
                        }
                    });
                }
            },
            getCommission: function () {
                var mid = MerchantRegister.privateMethod.getMerchantId();
                $.ajax({
                    url: 'GetCommission',
                    type: 'POST',
                    data: { "mid": mid },
                    success: function (data) {
                        app.modal.show(data);
                    },
                    error: function (err) {
                        MerchantRegister.privateMethod.ajaxError(err, MerchantRegister.properties.alerts.terms);
                    }
                });
            }
        },
        stateMachine: {
            currenState: 0,
            queue: [],
            getCurrentState: function () {
                var cs = parseInt($('#MerchantState').val());
                MerchantRegister.stateMachine.currenState = cs === NaN ? 0 : cs;
            },
            setDefaultQueue: function () {
                var states = MerchantRegister.properties.registerationState;
                MerchantRegister.stateMachine.queue = [
                    states.New,
                    states.Info,
                    states.Services,
                    states.Owners,
                    states.Outlets,
                    states.BankAccount,
                    states.Files,
                    states.Terms,
                    states.Complete
                ];
            },
            addState: function (afterState, newState) {
                if (MerchantRegister.stateMachine.queue.indexOf(newState) < 0) {
                    var position = MerchantRegister.stateMachine.queue.indexOf(afterState) + 1;
                    MerchantRegister.stateMachine.queue.splice(position, 0, newState);
                }
            },
            removeState: function (state) {
                var position = MerchantRegister.stateMachine.queue.indexOf(state);
                if (position >= 0) {
                    MerchantRegister.stateMachine.queue.splice(position, 1);
                }
            },
            checkState: function () {
                MerchantRegister.privateMethod.checkServices();
                var states = MerchantRegister.properties.registerationState;
                var sMachine = MerchantRegister.stateMachine;
                var nextState = sMachine.queue[sMachine.queue.indexOf(sMachine.currenState) + 1];
                if (sMachine.currenState == states.Complete) {
                    nextState = sMachine.currenState;
                }
                switch (nextState) {
                    case states.New:
                        MerchantRegister.stateMachine.state_new();
                        break;
                    case states.Services:
                        MerchantRegister.stateMachine.state_services();
                    case states.Info:
                        MerchantRegister.stateMachine.state_info();
                        break;
                    case states.Owners:
                        MerchantRegister.stateMachine.state_owner();
                        break;
                    case states.Outlets:
                        MerchantRegister.stateMachine.state_outlet();
                        break;
                    case states.BankAccount:
                        MerchantRegister.stateMachine.state_bank();
                        break;
                    case states.Files:
                        MerchantRegister.stateMachine.state_file();
                        break;
                    case states.Channel:
                        MerchantRegister.stateMachine.state_channel();
                        break;
                    case states.Complete:
                        MerchantRegister.stateMachine.state_complete();
                        break;
                    case states.Terms:
                        MerchantRegister.stateMachine.state_terms();
                        break;
                    default:
                        console.error("state is not supported: " + nextState);
                        break;
                }
            },
            state_new: function () {
                MerchantRegister.util.collapse.show(MerchantRegister.properties.collapses.info);
            },
            state_info: function () {
                MerchantRegister.util.collapse.show(MerchantRegister.properties.collapses.info);
            },
            state_services: function () {
                MerchantRegister.util.collapse.show(MerchantRegister.properties.collapses.services);
            },
            state_owner: function () {
                MerchantRegister.util.collapse.show(MerchantRegister.properties.collapses.owner);
            },
            state_outlet: function () {
                MerchantRegister.util.collapse.show(MerchantRegister.properties.collapses.outlet);
            },
            state_bank: function () {
                MerchantRegister.util.collapse.show(MerchantRegister.properties.collapses.bank);
            },
            state_file: function () {
                MerchantRegister.util.collapse.show(MerchantRegister.properties.collapses.file);
            },
            state_channel: function () {
                MerchantRegister.util.collapse.show(MerchantRegister.properties.collapses.channel);
            },
            state_terms: function () {
                MerchantRegister.util.collapse.show(MerchantRegister.properties.collapses.terms);
            },
            state_complete: function () {
                MerchantRegister.util.collapse.hide(MerchantRegister.properties.collapses.terms);
            }
        },
        privateMethod: {
            ajaxInfoSuccess: function (data) {
                if (data.isSuccessful) {
                    $('#' + MerchantRegister.properties.mid).val(data.id);
                    MerchantRegister.privateMethod.setFormNumber(data.message);
                    MerchantRegister.privateMethod.setFormName();
                    MerchantRegister.stateMachine.currenState = MerchantRegister.properties.registerationState.Info;
                    MerchantRegister.stateMachine.checkState();
                } else {
                    MerchantRegister.util.alert.show(MerchantRegister.properties.alerts.info, MerchantRegister.privateMethod.makeHtmlFromAjaxResponse(data));
                }
            },
            ajaxServicesSuccess: function (data) {
                if (data.isSuccessful) {
                    MerchantRegister.stateMachine.currenState = MerchantRegister.properties.registerationState.Services;
                    MerchantRegister.stateMachine.checkState();
                } else {
                    MerchantRegister.util.alert.show(MerchantRegister.properties.alerts.services, MerchantRegister.privateMethod.makeHtmlFromAjaxResponse(data));
                }
            },
            setFormName: function () {
                $('#formNameSpan').text(MerchantRegister.privateMethod.getMerchantName());
            },
            setFormNumber: function (number) {
                $('#formIdSpan').text(number);
            },
            ajaxAddOwnerSuccess: function (data) {
                if (data.isSuccessful) {
                    app.modal.hide();
                    MerchantRegister.privateMethod.ownerInitTable();
                } else {
                    MerchantRegister.util.alert.show(MerchantRegister.properties.alerts.owner, MerchantRegister.privateMethod.makeHtmlFromAjaxResponse(data));
                }
            },
            ajaxRemoveOwnerSuccess: function (data) {
                if (data.isSuccessful) {
                    app.modal.hide();
                    MerchantRegister.privateMethod.ownerInitTable();
                } else {
                    MerchantRegister.util.alert.show(MerchantRegister.properties.alerts.ownerMaster, MerchantRegister.privateMethod.makeHtmlFromAjaxResponse(data));
                }
            },
            ajaxAddOutletSuccess: function (data) {
                if (data.isSuccessful) {
                    app.modal.hide();
                    MerchantRegister.privateMethod.outletInitTable();
                } else {
                    MerchantRegister.util.alert.show(MerchantRegister.properties.alerts.outlet, MerchantRegister.privateMethod.makeHtmlFromAjaxResponse(data));
                }
            },
            ajaxRemoveOutletSuccess: function (data) {
                if (data.isSuccessful) {
                    app.modal.hide();
                    MerchantRegister.privateMethod.outletInitTable();
                } else {
                    MerchantRegister.util.alert.show(MerchantRegister.properties.alerts.outletMaster, MerchantRegister.privateMethod.makeHtmlFromAjaxResponse(data));
                }
            },
            ajaxBankSuccess: function (data) {
                if (data.isSuccessful) {
                    MerchantRegister.stateMachine.currenState = MerchantRegister.properties.registerationState.BankAccount;
                    MerchantRegister.stateMachine.checkState();
                } else {
                    MerchantRegister.util.alert.show(MerchantRegister.properties.alerts.bank, MerchantRegister.privateMethod.makeHtmlFromAjaxResponse(data));
                }
            },
            ajaxChannelSuccess: function (data) {
                if (data.isSuccessful) {
                    MerchantRegister.stateMachine.currenState = MerchantRegister.properties.registerationState.Channel;
                    MerchantRegister.stateMachine.checkState();
                } else {
                    MerchantRegister.util.alert.show(MerchantRegister.properties.alerts.channel, MerchantRegister.privateMethod.makeHtmlFromAjaxResponse(data));
                }
            },
            ajaxError: function (err, alertSection) {
                switch (err.status) {
                    case 401:
                        window.location = '/Account/Logout';
                        break;
                    case 403:
                        window.location = '/Account/Logout';
                        break;
                    default:
                        MerchantRegister.util.alert.show(alertSection, '<div>' + err.statusText + '</div>');
                }
            },
            ajaxRejectSuccess: function (data) {
                if (data.isSuccessful) {
                    $('#' + MerchantRegister.properties.modals.reject).modal('hide');
                    window.location = app.properties.BaseURL + "Merchant/Inbox";
                } else {
                    MerchantRegister.util.alert.show(MerchantRegister.properties.alerts.reject, MerchantRegister.privateMethod.makeHtmlFromAjaxResponse(data));
                }
            },
            showOwnerModal: function () {
                MerchantRegister.method.getOwner(null);
            },
            showOutletModal: function () {
                MerchantRegister.method.getOutlet(null);
            },
            validateProperty: function (propId, validations) {

                if (validations.includes("Required")) {
                    MerchantRegister.validations.required(propId);
                }
            },
            makeHtmlFromAjaxResponse: function (data) {
                var alertContent = '';
                if (data.message) {
                    alertContent += '<p>' + data.message + '</p>';
                }
                if (data.validations.length > 0) {
                    alertContent += '<ul>';
                    for (var i = 0; i < data.validations.length; i++) {
                        alertContent += '<li>' + data.validations[i].field + ' ' + data.validations[i].description + '</li>';
                    }
                    alertContent += '</ul>'
                }
                return alertContent;
            },
            makeStringFromAjaxResponse: function (data) {
                var alertContent = '';
                if (data.message) {
                    alertContent += data.message + ' ';
                }
                if (data.validations.length > 0) {
                    for (var i = 0; i < data.validations.length; i++) {
                        alertContent += '*' + data.validations[i].field + ' ' + data.validations[i].description + ' ';
                    }
                }
                return alertContent;
            },
            getMerchantId: function () {
                return $('#' + MerchantRegister.properties.mid).val();
            },
            getMerchantName: function () {
                return $('#RegisteredBusiness').val();
            },
            ownerInitTable: function () {
                var mid = MerchantRegister.privateMethod.getMerchantId();
                if (!MerchantRegister.properties.grids.owner.table) {
                    MerchantRegister.properties.grids.owner.table = $('#' + MerchantRegister.properties.grids.owner.id).DataTable({
                        ajax: {
                            url: "GetOwners",
                            type: "POST",
                            dataSrc: MerchantRegister.util.afterGridAjaxSuccess,
                            data: { id: mid },
                            beforeSend: function () {
                                app.utils.showLoading();
                            },
                            error: function (err) {
                                app.utils.hideLoading();
                                MerchantRegister.privateMethod.ajaxError(err, MerchantRegister.properties.alerts.ownerMaster);
                            }
                        },
                        autoWidth: false,
                        searching: false,
                        paging: false,
                        ordering: false,
                        info: false,
                        language: {
                            zeroRecords: 'No any Owner or Director'
                        },
                        columns: [
                            {
                                data: null,
                                orderable: false,
                                searchable: false,
                                render: MerchantRegister.privateMethod.ownerActionRender
                            },
                            { data: "id", visible: false },
                            { data: "ownerName" },
                            { data: "ownerDesignation" },
                            { data: "ownerIdentityNo" },
                            { data: "ownerMobile" }
                        ]
                    });
                } else {
                    MerchantRegister.properties.grids.owner.table.ajax.reload();
                }
            },
            ownerActionRender: function (data, type, row, meta) {
                return `<a class="btn" title="Remove" onclick="MerchantRegister.method.removeOwner('${row.id}')"><i class="fas fa-user-slash text-primary"></i></a>
<a class="btn" title="Edit" onclick="MerchantRegister.method.getOwner('${row.id}')"><i class="far fa-edit text-primary"></i></a>`
            },
            outletInitTable: function () {
                var mid = MerchantRegister.privateMethod.getMerchantId();
                if (!MerchantRegister.properties.grids.outlet.table) {
                    MerchantRegister.properties.grids.outlet.table = $('#' + MerchantRegister.properties.grids.outlet.id).DataTable({
                        ajax: {
                            url: "GetOutlets",
                            type: "POST",
                            dataSrc: MerchantRegister.util.afterGridAjaxSuccess,
                            data: { id: mid },
                            beforeSend: function () {
                                app.utils.showLoading();
                            },
                            error: function (err) {
                                app.utils.hideLoading();
                                MerchantRegister.privateMethod.ajaxError(err, MerchantRegister.properties.alerts.outletMaster);
                            }
                        },
                        autoWidth: false,
                        searching: false,
                        paging: false,
                        ordering: false,
                        info: false,
                        language: {
                            zeroRecords: 'No any Outlet'
                        },
                        columns: [
                            {
                                data: null,
                                orderable: false,
                                searchable: false,
                                render: MerchantRegister.privateMethod.outletActionRender
                            },
                            { data: "id", visible: false },
                            { data: "oName" },
                            { data: "oTown" },
                            { data: "oTelNo" },
                            { data: "oContactPerson" }
                        ]
                    });
                } else {
                    MerchantRegister.properties.grids.outlet.table.ajax.reload();
                }
            },
            outletActionRender: function (data, type, row, meta) {
                return `<a class="btn" title="Remove" onclick="MerchantRegister.method.removeOutlet('${row.id}')"><i class="fas fa-store-alt-slash text-primary"></i></a>
<a class="btn" title="Edit" onclick="MerchantRegister.method.getOutlet('${row.id}')"><i class="far fa-edit text-primary"></i></a>`
            },
            ajaxUploadSuccess: function (id, data) {
                if (data.isSuccessful) {
                    MerchantRegister.fileToggle(id, data);
                } else {
                    app.validation.alert.show(id, MerchantRegister.privateMethod.makeStringFromAjaxResponse(data));
                }
            },
            ajaxUploadError: function (id, error) {
                app.validation.alert.show(id, error.statusText);
            },
            checkServices: function () {
                var formData = new FormData(document.getElementById('formServices'));
                var vasCount = 0;
                var otherCount = 0;
                var m1payCount = 0;
                for (var key of formData.keys()) {
                    if (key.includes('Vas')) {
                        vasCount++;
                    } else {
                        otherCount++;
                    }
                    if (key.includes('M1Pay')) {
                        m1payCount++;
                    }
                }
                if (vasCount && !otherCount) {
                    MerchantRegister.privateMethod.showVasFiles();
                } else {
                    MerchantRegister.privateMethod.showOtherVasFiles();
                }
                if (m1payCount) {
                    MerchantRegister.privateMethod.showChannelSection();
                } else {
                    MerchantRegister.privateMethod.hideChannelSection();
                }
            },
            showVasFiles: function () {
                $('#CompanyRegistrationSearchUpHolder').parent().show();
                $('#IdentificationDocumentsUpHolder').parent().show();
                $('#BankStatementUpHolder').parent().hide();
                $('#ApplicantPhotoUpHolder').parent().hide();
                $('#CtosOfBoardUpHolder').parent().hide();
                $('#OtherDocumentUpHolder').parent().show();
            },
            showOtherVasFiles: function () {
                $('#CompanyRegistrationSearchUpHolder').parent().show();
                $('#IdentificationDocumentsUpHolder').parent().show();
                $('#BankStatementUpHolder').parent().show();
                $('#ApplicantPhotoUpHolder').parent().show();
                $('#CtosOfBoardUpHolder').parent().show();
                $('#OtherDocumentUpHolder').parent().show();
            },
            showChannelSection: function () {
                MerchantRegister.stateMachine.addState(MerchantRegister.properties.registerationState.BankAccount, MerchantRegister.properties.registerationState.Channel);
                $('#headerChannel').parent().show();
            },
            hideChannelSection: function () {
                MerchantRegister.stateMachine.removeState(MerchantRegister.properties.registerationState.Channel);
                $('#headerChannel').parent().hide();
            },
            showRejectModal: function () {
                $('#rejectModal').modal('show');
            },
            showApproveModal: function () {
                app.confirmModal.confirmAction = MerchantRegister.method.approve;
                app.confirmModal.setTitle("Approve Merchant");
                app.confirmModal.setBody("By approving the merchant, it will proceed for next higher approval. are you sure?");
                app.confirmModal.setParam(MerchantRegister.privateMethod.getMerchantId());
                app.confirmModal.popUp();
            },
            showAcceptModal: function () {
                app.confirmModal.confirmAction = MerchantRegister.method.accept;
                app.confirmModal.setTitle("Accept Merchant");
                app.confirmModal.setBody("By accepting the merchant, we will notify them that they are approved. are you sure?");
                app.confirmModal.setParam(MerchantRegister.privateMethod.getMerchantId());
                app.confirmModal.popUp();
            },
            editCommission: function () {
                var url = app.properties.BaseURL + "Merchant/EditCommission?mid=" + MerchantRegister.privateMethod.getMerchantId();
                window.location = url;
            }
        },
        event: {
            register: function () {
                app.event.customFile();
                app.event.select2();
                app.utils.initLoading();

                var events = MerchantRegister.event;

                events.registerInfoSaveBtn();

                events.registerServicesSaveBtn();

                events.registerOwnerCallToAddBtn();
                events.registerOwnerSaveBtn();
                events.registerOwnerContinueBtn();

                events.registerOutletCallToAddBtn();
                events.registerOutletSaveBtn();
                events.registerOutletContinueBtn();

                events.registerShowCollapses();
                events.registerHideOwnerModal();
                events.registerHideOutletModal();

                events.registerBankSaveBtn();

                events.channelSaveBtn();

                events.registerFileSaveBtn();

                events.registerCompleteBtn();
                events.rejectBtn();
                events.rejectSaveBtn();
                events.approveBtn();
                events.acceptBtn();
                events.commissionBtn();
                events.showCommissionBtn();

                events.registerTermsContinue();

                MerchantRegister.stateMachine.setDefaultQueue();
                MerchantRegister.stateMachine.getCurrentState();
                MerchantRegister.stateMachine.checkState();
            },
            registerInfoSaveBtn: function () {
                $('#infoBtnSave').on('click', function () {
                    MerchantRegister.method.registerInfo();
                });
            },
            registerServicesSaveBtn: function () {
                $('#servicesBtnSave').on('click', function () {
                    MerchantRegister.method.registerServices();
                });
            },
            registerOwnerCallToAddBtn: function () {
                $('#ownerCallToAddBtn').on('click', function () {
                    MerchantRegister.privateMethod.showOwnerModal();
                })
            },
            registerOwnerSaveBtn: function () {
                $('body').on('click', '#ownerSaveBtn', function () {
                    MerchantRegister.method.addOwner();
                });
            },
            registerOwnerContinueBtn: function () {
                $('#ownerContinueBtn').on('click', function () {
                    MerchantRegister.method.continueOwner();
                });
            },
            registerOutletCallToAddBtn: function () {
                $('#outletCallToAddBtn').on('click', function () {
                    MerchantRegister.privateMethod.showOutletModal();
                })
            },
            registerOutletSaveBtn: function () {
                $('body').on('click', '#outletSaveBtn', function () {
                    MerchantRegister.method.addOutlet();
                });
            },
            registerOutletContinueBtn: function () {
                $('#outletContinueBtn').on('click', function () {
                    MerchantRegister.method.continueOutlet();
                });
            },
            registerShowCollapses: function () {
                $('#' + MerchantRegister.properties.collapses.owner).on('show.bs.collapse', function () {
                    MerchantRegister.privateMethod.ownerInitTable();
                });
                $('#' + MerchantRegister.properties.collapses.outlet).on('show.bs.collapse', function () {
                    MerchantRegister.privateMethod.outletInitTable();
                });
            },
            registerHideOwnerModal: function () {
                $('#' + MerchantRegister.properties.modals.owner).on('hidden.bs.modal', function (e) {
                    MerchantRegister.form.clearFormFields(MerchantRegister.properties.forms.owner);
                    MerchantRegister.util.alert.clear(MerchantRegister.properties.alerts.owner);
                })
            },
            registerHideOutletModal: function () {
                $('#' + MerchantRegister.properties.modals.outlet).on('hidden.bs.modal', function (e) {
                    MerchantRegister.form.clearFormFields(MerchantRegister.properties.forms.outlet);
                    MerchantRegister.util.alert.clear(MerchantRegister.properties.alerts.outlet);
                })
            },
            registerBankSaveBtn: function () {
                $('#bankSaveBtn').on('click', function () {
                    MerchantRegister.method.saveBankAccount();
                });
            },
            registerFileSaveBtn: function () {
                $('#fileSaveBtn').on('click', function () {
                    MerchantRegister.method.continueFile();
                });
            },
            registerTermsContinue: function () {
                $('#termSaveBtn').on('click', function () {
                    MerchantRegister.method.continueTerms();
                });
            },
            registerCompleteBtn: function () {
                $('#completeBtn').on('click', function () {
                    MerchantRegister.method.complete();
                });
            },
            channelSaveBtn: function () {
                $('#channelSaveBtn').on('click', function () {
                    MerchantRegister.method.saveChannel();
                });
            },
            rejectBtn: function () {
                $('#rejectBtn').on('click', function () {
                    MerchantRegister.privateMethod.showRejectModal();
                });
            },
            approveBtn: function () {
                $('#approveBtn').on('click', function () {
                    MerchantRegister.privateMethod.showApproveModal();
                });
            },
            acceptBtn: function () {
                $('#acceptBtn').on('click', function () {
                    MerchantRegister.privateMethod.showAcceptModal();
                });
            },
            rejectSaveBtn: function () {
                $('#rejectSaveBtn').on('click', function () {
                    MerchantRegister.method.reject();
                });
            },
            commissionBtn: function () {
                $('#commissionBtn').on('click', function () {
                    MerchantRegister.privateMethod.editCommission();
                });
            },
            showCommissionBtn: function () {
                $('#showCommissionBtn').on('click', function () {
                    MerchantRegister.method.getCommission();
                });
            }
        },
        form: {
            isValid: true,
            collectFormObject: function (formId, model) {
                for (prop in model) {
                    if (Object.prototype.hasOwnProperty.call(model, prop)) {
                        var target = document.querySelector('#' + formId + ' #' + prop);
                        if (target) {
                            switch (target.type) {
                                case 'text':
                                    model[prop] = target.value;
                                    break;
                                case 'textarea':
                                    model[prop] = target.value;
                                    break;
                                case 'fieldset':
                                    model[prop] = target.querySelector('input:checked') === null ? null : target.querySelector('input:checked').value;
                                    break;
                                default:
                                    model[prop] = null;

                            }
                        }
                    }
                }
                return model;
            },
            clearFormFields: function (formId) {
                var form = document.getElementById(formId);
                if (form) {
                    form.reset();
                }
            },
            getAntiForgeryHeader: function () {
                var headers = {};
                var token = $('[name=__RequestVerificationToken]').val();
                headers["__RequestVerificationToken"] = token;
                console.log(headers);
                return headers;
            },
            validate: function (formId, model) {
                MerchantRegister.form.isValid = true;
                for (prop in model) {
                    if (Object.prototype.hasOwnProperty.call(model, prop)) {
                        var target = document.querySelector('#' + formId + ' #' + prop);
                        var validationsStr = target?.getAttribute("data-validations");
                        if (validationsStr) {
                            var validations = validationsStr.split(',');
                            MerchantRegister.privateMethod.validateProperty(prop, validations);
                        }
                    }
                }
                return MerchantRegister.form.isValid;
            }
        },
        util: {
            alert: {
                hide: function (alertId) {
                    $('#' + alertId).html('').removeClass().addClass("d-none");
                },
                show: function (alertId, htmlContent) {
                    $('#' + alertId).html(htmlContent).removeClass().addClass("alert alert-danger");
                    MerchantRegister.util.alert.gotoAlert(alertId);
                },
                gotoAlert: function (alertId) {
                    $([document.documentElement, document.body]).animate({
                        scrollTop: $('#' + alertId).offset().top
                    }, 500);
                },
                showText: function (alertId, alertText) {
                    $('#' + alertId).text(alertText).removeClass().addClass("alert alert-danger");
                    MerchantRegister.util.alert.gotoAlert(alertId);
                },
                clear: function (alertId) {
                    app.validation.alert.reset();
                    MerchantRegister.util.alert.hide(alertId);
                }
            },
            collapse: {
                show: function (collapseId) {
                    $('#' + collapseId).collapse('show');
                },
                hide: function (collapseId) {
                    $('#' + collapseId).collapse('hide');
                }
            },
            showValidationError: function (propId, text) {
                $('#' + propId + 'Validation').html(text).removeClass('d-none');
            },
            hideValidationError: function (propId) {
                $('#' + propId + 'Validation').html('').addClass('d-none');
            },
            afterGridAjaxSuccess: function (data) {
                app.utils.hideLoading();
                return data;
            },
        },
        validations: {
            required: function (propId) {
                if ($('#' + propId).val()) {
                    MerchantRegister.util.hideValidationError(propId);
                } else {
                    MerchantRegister.form.isValid = false;
                    MerchantRegister.util.showValidationError(propId, 'the field is required');
                }
            },
            isFileValid: function (id) {
                //MerchantRegister.util.hideValidationError(id);
                var validExtensions = ['jpg', 'jpeg', 'pdf', 'png'];
                var fileName = $('#' + id).val();
                if (fileName.length == 0) {
                    app.validation.alert.show(id, "Please select a file");
                    return false;
                } else {
                    var extension = fileName.replace(/^.*\./, '');
                    if ($.inArray(extension, validExtensions) == -1) {
                        app.validation.alert.show(id, `Only These types are allowed: ${validExtensions}`);
                        return false;
                    }
                }

                return true;
            }
        },
        fileHandler: function (id, isUpload) {
            if (isUpload) {
                MerchantRegister.method.uploadFile(id);
            } else {
                MerchantRegister.method.removeFile(id);
            }
        },
        fileToggle: function (id, data) {
            app.validation.alert.hide(id);
            $('#' + id + 'Holder' + ' a').attr('href', data.id);
            $('#' + id + 'UpHolder').toggleClass('d-none');
            $('#' + id + 'Holder').toggleClass('d-none');
        }
    }
}
$(function () {
    MerchantRegister.event.register();
});