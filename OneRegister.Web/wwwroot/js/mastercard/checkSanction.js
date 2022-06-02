if (typeof (checkSanction) === 'undefined') {
    var checkSanction = {
        method: {
            inquiry: function () {
                checkSanction.method.resetAllStatusIcon();
                checkSanction.method.clearResult();
                app.form.submit('mainForm', 'CheckSanction', checkSanction.method.showResult)
            },
            showResult: function (data) {

                let messageInJsonFormat = JSON.parse(data.message);
                if (messageInJsonFormat.result === status.consider) {
                    let recordsDetail = messageInJsonFormat.properties.records;
                    $.each(recordsDetail, function (index, value) {
                        var relatedUrls = "";
                        var relatedUrlsHTML = "";
                        if (value.related_urls) {
                            relatedUrls = value.related_urls.split(',');
                            $.each(relatedUrls, (index, value) => {
                                relatedUrlsHTML += `<li><a href="${value}">${value}</a></li>`
                            });
                        }

                        var last_updated_utcStr = "";
                        if (value.last_updated_utc) {
                            var dateTime = new Date(value.last_updated_utc);
                            var date = dateTime.toISOString().split('T')[0];
                            var time = dateTime.toISOString().split('T')[1].substring(0, 8)
                            last_updated_utcStr = `${date} ${time}`
                        }
                        $('#match-container').append(`
<h5 class="match-container-header">
<button id="match-container-toggle-btn-${index + 1}" class="btn btn-secondary btn-sm match-container-toggle-btn mr-2 px-1 py-0" type="button" data-toggle="collapse" data-target="#match-container-${index + 1}" aria-expanded="true" aria-controls="match-container-${index + 1}"><i class="fas fa-minus"></i></button>
Match ${index + 1}</h5>
<div class="match-item multi-collapse collapse show" id="match-container-${index + 1}">
            <div class="row">
                <div class="col-md-6">
                    <p class="match-label">Date Of Birth</p>
                    <p class="match-value">${value.entity_fields_dob}</p>
                </div>
                <div class="col-md-6">
                    <p class="match-label">Name</p>
                    <p class="match-value">${value.name}</p>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <p class="match-label">Strength of match</p>
                    <p class="match-value">${value.match_types}</p>
                </div>
                <div class="col-md-6">
                    <p class="match-label">Record last updated</p>
                    <p class="match-value">${last_updated_utcStr}</p>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <p class="match-label">Related urls</p>
                    <div class="match-value">
                        <ul>${relatedUrlsHTML}</ul>
                    </div>
                </div>
                <div class="col-md-6">
                    <p class="match-label">Watchlist types</p>
                    <p class="match-value">${value.types}</p>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6">
                    <p class="match-label">Source</p>
                    <p class="match-value">${value.sources}</p>
                </div>
                <div class="col-md-6">
                    <p class="match-label">Country</p>
                    <p class="match-value">${value.entity_fields_countries}</p>
                </div>
            </div>
        </div>`);


                    });
                    $('.match-container-toggle-btn').on('click', function () {
                        let hasPlus = $(this).find('i').hasClass('fa-plus');
                        if (hasPlus) {
                            $(this).find('i').removeClass().addClass('fas fa-minus');
                        } else {
                            $(this).find('i').removeClass().addClass('fas fa-plus');
                        }
                    })
                }

                let breakdownItems = messageInJsonFormat.breakdown;

                if (breakdownItems !== undefined) {
                    var targetClass = "";
                    $.each(breakdownItems, function (type, item) {
                        switch (type) {
                            case "legal_and_regulatory_warnings":
                                targetClass = "legalAndRegulatoryWarning";
                                break;
                            case "politically_exposed_person":
                                targetClass = "politicallyExposedPerson";
                                break;
                            case "sanction":
                                targetClass = "sanction";
                                break;
                        }

                        if (item.result === status.clear) {
                            checkSanction.method.toggleStatusCssClass(`${targetClass}`, "fa-check-circle text-success")

                        } else if (item.result === status.consider) {
                            checkSanction.method.toggleStatusCssClass(`${targetClass}`, "fa-minus-circle text-warning")
                        } else {
                            checkSanction.method.toggleStatusCssClass(`${targetClass}`, "fa-question-circle text-muted")
                        }
                    })
                } else {
                    checkSanction.method.resetAllStatusIcon();
                }
                $('#sanction-result').jsonViewer(JSON.parse(data.message));
            },
            clearResult: function () {
                $('#sanction-result').html('');
                $('#match-container').html('');
            },
            resetAllStatusIcon: function () {
                $('.status-container-item i').removeClass().addClass('mr-1 header-icon fa-2x fas fa-question-circle text-muted');
            },
            toggleStatusCssClass: function (targetClass, newCssClasses) {
                var defaultCssClass = `mr-1 header-icon fa-2x fas `
                $(`.${targetClass}List .header-icon`).removeClass().addClass(`${defaultCssClass} ${newCssClasses}`)
            }
        },
        event: {
            inquiryBtnClick: function () {
                $('#inquiryBtn').on('click', function () {
                    checkSanction.method.inquiry();
                })
            },
            register: function () {
                var e = checkSanction.event;

                e.inquiryBtnClick();
            }
        }
    }
}
let status = {
    clear: "clear",
    consider: "consider"
}
$(function () {
    checkSanction.event.register();
});

