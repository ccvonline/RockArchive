﻿///
///  Giving Page
/// --------------------------------------------------

const decepticon = function () {

    let decepticonMult;
    let decepticonVar;

    let that = {};

    that.attack = () => {

        $('#hfDecepticon').val(decepticonMult * decepticonVar);
        $('#hfDecepticonMult').val(decepticonMult);
        
    }

    that.mobilize = () => {
        decepticonMult = Math.floor(Math.random() * Math.floor(10));
        decepticonVar = 0;
        setInterval(function () {
            decepticonVar++;
        }, 1000);
    }

    return that;
}

let megatron = new decepticon;

const handleSubmit = function (e) {

    // Disable processing submit button after its clicked to prevent duplicate submits
    $('#btnConfirmNext').attr('disabled', 'disabled');

    //Attack the Autobots! 
    megatron.attack();


    return true;
}

// Components that persist through postbacks
function pageLoad() {

    //Mobilize the Decepticons
    megatron.mobilize();

    //
    // Transaction Panel
    //

    // Set style of whichever bank account type radio button is checked
    $('#rblAccountType').on('change', function () {
        $('input[type="radio"]:checked').parents('label').addClass('btn-primary');
        $('input[type="radio"]:not(:checked)').parents('label').removeClass('btn-primary');
    });

    // on postbacks, check for rblAccountType selected value and set button state if something is checked
    if ($('input[type="radio"]').is(':checked')) {
        $('input[type="radio"]:checked').parents('label').addClass('btn-primary');
        $('input[type="radio"]:not(:checked)').parents('label').removeClass('btn-primary');
    } 

    // Disable progress buttons if they are not in a complete state
    // Not sure why, but disabling at the ASP control level breaks javascript...which is why its here
    if (!$('#btnProgressAmount').hasClass( 'complete' )) {
        $('#btnProgressAmount').attr('disabled', 'disabled');
    }

    if (!$('#btnProgressPerson').hasClass('complete')) {
        $('#btnProgressPerson').attr('disabled', 'disabled');
    }

    if (!$('#btnProgressPayment').hasClass('complete')) {
        $('#btnProgressPayment').attr('disabled', 'disabled');
    }

    // Validate amount input
    $('#nbAmount').on('input', function () {
        // Remove white space and split at the decimal
        var amount = this.value.replace(/\s/gi, '').split('.');

        // format first element of array
        // remove non number characters, keep only 7 numbers, and then format with ,
        var formattedAmount = amount[0].replace(/[^0-9.]/g, "").substring(0, 7).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").trim();

        // Check for decimal amount and if exists add it back to amount
        // if array more than one element, format the second element with 1 or 2 characters and ignore the rest
        if (amount.length > 1) {
            formattedAmount = formattedAmount + '.' + amount[1].replace(/[^0-9]/g, "").substring(0, 2).trim();
        }


        if (!formattedAmount || formattedAmount === '0' || formattedAmount === '0.') {
            $(this).parents('div.amount-wrapper').addClass('has-error');
        } else {
            // set the new value
            this.value = '$' + formattedAmount;
    
            $(this).parents('div.amount-wrapper').removeClass('has-error');
        }
    });

    // Validate accounts input
    $('#ddlAccounts').on('change', function () {
        var fund = $(this).find(':selected').val();

        if ((!fund || fund === '-1')) {
            $(this).parents('div.accounts-wrapper').addClass('has-error');
        } else {
            $(this).parents('div.accounts-wrapper').removeClass('has-error');
        }
    });

    // Validate Schedule Frequency
    $('#ddlScheduleFrequency').on('change', function () {
        var frequency = $(this).find(':selected').val();

        if ((!frequency || frequency === '-1')) {
            $(this).parents('div.schedule-transaction-wrapper').addClass('has-error');
        } else {
            $(this).parents('div.schedule-transaction-wrapper').removeClass('has-error');
        }
    });

    // Validate schedule start date picker
    $('#dpScheduledTransactionStartDate').on('change', function () {
        if (/^02\/(?:[01]\d|2\d)\/(?:0[048]|[13579][26]|[2468][048])|(?:0[13578]|10|12)\/(?:[0-2]\d|3[01])\/\d{2}|(?:0[469]|11)\/(?:[0-2]\d|30)\/\d{2}|02\/(?:[0-1]\d|2[0-8])\/\d{2}$/.test(this.value)) {
            $('#dpScheduledTransactionStartDate').parents('div.schedule-date-wrapper').removeClass('has-error');
        } else {
            $('#dpScheduledTransactionStartDate').parents('div.schedule-date-wrapper').addClass('has-error');
        }
    });

    // Enable / Disable recurring transaction
    $('#tglScheduledTransaction').on('change', function () {
        if (!$('#tglScheduledTransaction').is(':checked')) {
            $('#hfIsScheduledTransaction').attr('value', 'false');
            $('#dpScheduledTransactionStartDate').parents('div.schedule-date-wrapper').removeClass('has-error');
            $('#ddlScheduleFrequency').parents('div.schedule-transaction-wrapper').removeClass('has-error');
        } else {
            $('#hfIsScheduledTransaction').attr('value', 'true');
        }
    });

    // Validate email input
    $('#tbEmail').on('input', function () {
        if (!/^\w([\.-]?\w)*@\w([\.-]?\w)*(\.\w{2,15})+$/.test($(this).val())) {
            $(this).parents('div.form-group').addClass('has-error');
        } else {
            $(this).parents('div.form-group').removeClass('has-error');
        }
    });

    // Validate generic text input
    $('.required').on('input', function () {
        if (!this.value) {
            $(this).parents('div.form-group').addClass('has-error');
        } else {
            $(this).parents('div.form-group').removeClass('has-error');
        }
    });

    // Validate saved payment accounts
    $('#ddlSavedPaymentAccounts').on('change', function () {
        var account = $(this).find(':selected').val();

        if ((!account || account === '-1')) {
            $(this).parents('div.savedpayment-wrapper').addClass('has-error');
        } else {
            $(this).parents('div.savedpayment-wrapper').removeClass('has-error');
        }
    });

    // Validate / Format Credit card input
    $('#nbCreditCard').on('input', function () {
        // Remove spaces from value for validation
        var creditCardNumber = this.value.replace(/[^0-9]/g, '');

        // Test expressions for various credit cards
        var visaRegEx = /^(?:4[0-9]{12}(?:[0-9]{3})?)$/;
        var mastercardRegEx = /^(?:4[0-9]{12}(?:[0-9]{3})?|[25][1-7][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$/;
        var amexpRegEx = /^(?:3[47][0-9]{13})$/;
        var discovRegEx = /^(?:6(?:011|5[0-9][0-9])[0-9]{12})$/;

        var isValid = false;
        // Test credit card number against each card type
        if (visaRegEx.test(creditCardNumber)) {
            isValid = true;
        } else if (mastercardRegEx.test(creditCardNumber)) {
            isValid = true;
        } else if (amexpRegEx.test(creditCardNumber)) {
            isValid = true;
        } else if (discovRegEx.test(creditCardNumber)) {
            isValid = true;
        }

        // Reformat number with spaces
        this.value = creditCardNumber.replace(/[^0-9]/g, "").replace(/\W/gi, '').replace(/(.{4})/g, '$1 ').trim();

        // Update display class for valid or invalid input
        if (!isValid) {
            $(this).parents('div.creditcard-wrapper').addClass('has-error');
        } else {
            $(this).parents('div.creditcard-wrapper').removeClass('has-error');
        }
    });

    // Validate experiation month input
    $('#monthDropDownList').on('change', function () {
        if (!$(this).find(':selected').val()) {
            $(this).parents('div.form-group').addClass('has-error');
        } else {
            // only remove error if year drop down is also selected
            if ($('#yearDropDownList_').find(':selected').val()) {
                $(this).parents('div.form-group').removeClass('has-error');
            }
        }
    });

    // Validate experation year input
    $('#yearDropDownList_').on('change', function () {
        if (!$(this).find(':selected').val()) {
            $(this).parents('div.form-group').addClass('has-error');
        } else {
            // only remove error if month drop down is also selected
            if ($('#monthDropDownList').find(':selected').val()) {
                $(this).parents('div.form-group').removeClass('has-error');
            }
        }
    });

    // Validate numbers only fields
    $('.numbers-only').on('input', function () {
        this.value = this.value.replace(/[^0-9]/g, "")

        if (this.value === '') {
            $(this).parents('div.form-group').addClass('has-error');
        } else {
            $(this).parents('div.form-group').removeClass('has-error');
        }
    });

    // Validate account type radio button list
    $('#rblAccountType').on('click', function () {
        if ($('#rblAccountType input[type=radio]:checked').val()) {
            $('#rblAccountType').parents('div.form-group').removeClass('has-error');
        } else {
            $('#rblAccountType').parents('div.form-group').addClass('has-error');
        }
    });

    // Validate saved payment account radio button list
    $('#rblSavedPaymentAccounts').on('click', function () {
        if ($('#rblSavedPaymentAccounts input[type=radio]:checked').val()) {
            $('#rblSavedPaymentAccounts').removeClass('has-error');
        } else {
            $('#rblSavedPaymentAccounts').addClass('has-error');
        }
    });


    //
    // Success Panel
    //

    // Save Payment Account Panel
    $('#tbSavePaymentAccountName').on('input', function () {
        // check if input has value
        if (!$(this).val()) {
            $(this).parents('div.form-group').addClass('has-error');
        } else {
            $(this).parents('div.form-group').removeClass('has-error');
        }

        // enable / disable save button
        setInputSaveButtonState();
    });

    // Schedule Recurring Transaction Panel

    // configure the schedule start for date pickers
    var tomorrowDate = new Date();
    tomorrowDate.setDate(tomorrowDate.getDate() + 1);

    $('#dpScheduledTransactionStartDate').datepicker({
        format: 'mm/dd/yyyy',
        startDate: tomorrowDate
    });

    // configured default date for date pickers
    $('#dpScheduledTransactionStartDate').datepicker('update', tomorrowDate);

    // Validate schedule drop down list
    $('#ddlSuccessScheduleFrequency').on('change', function () {
        if ($(this).find(':selected').val() && $(this).find(':selected').val() !== '-1') {
            // Calculate days until next payment
            var daysUntilNextPayment = null;
            switch ($(this).find(':selected').val()) {
                case '35711E44-131B-4534-B0B2-F0A749292362'.toLowerCase(): {
                    // Weekly
                    daysUntilNextPayment = 7;
                    break;
                }
                case '72990023-0D43-4554-8D32-28461CAB8920'.toLowerCase(): {
                    // Bi-Weekly
                    daysUntilNextPayment = 14;
                    break;
                }
                case '791C863D-2600-445B-98F8-3E5B66A3DEC4'.toLowerCase(): {
                    // Twice a Month
                    daysUntilNextPayment = 15;
                    break;
                }
                case '1400753C-A0F9-4A45-8A1D-81C98450BD1F'.toLowerCase(): {
                    // Monthly
                    daysUntilNextPayment = 31;
                    break;
                }
                case 'BF08EA03-C52A-4364-B142-12EBCA7CA14A'.toLowerCase(): {
                    // Quarterly
                    daysUntilNextPayment = 90;
                    break;
                }
                case '691BB8AB-5F96-4E88-847C-CB970D9E87FA'.toLowerCase(): {
                    // Twice A Year
                    daysUntilNextPayment = 180;
                    break;
                }
                case 'AC88C37A-901E-4CBB-947B-11348C208192'.toLowerCase(): {
                    // Yearly
                    daysUntilNextPayment = 365;
                    break;
                }
                default: {
                    // something went wrong, highlight error
                    $(this).parents('div.form-group').addClass('has-error');
                    break;
                }
            }

            if (daysUntilNextPayment) {
                // set hidden field so server has access to the value
                $('#hfSuccessScheduleStartDate').attr('value',moment().add(daysUntilNextPayment, 'days').calendar());

                // update schedule start date label
                $('#lblSuccessScheduleStartDate').html(moment().add(daysUntilNextPayment, 'days').calendar());
            }

            // Clear errors
            $(this).parents('div.form-group').removeClass('has-error');
        } else {
            // errors exist
            if ($(this).find(':selected').val() === '-1') {
                // if placeholder selected, clear schedule start date label
                $('#lblSuccessScheduleStartDate').html('');
            }
            $(this).parents('div.form-group').addClass('has-error');
        }

        // enable / disable save button
        setInputSaveButtonState();
    });

    // show/hide save button for success input form
    $('.toggle-input-form').on('click', function () {
        if ($('#tglSavePaymentAccount').is(':checked') || $('#tglSuccessScheduleTransaction').is(':checked')) {
            $('#btnSaveSuccessInputForm').removeClass('hidden');
        } else {
            $('#btnSaveSuccessInputForm').addClass('hidden');
        }

        // enable / disable save button
        setInputSaveButtonState();
    });
};

//
// Event Methods
// 

// Next button onClick
btnNext_OnClick = function (targetPanel) {
    if (targetPanel === 'pnlPerson') {
        // check if amount panel is completed
        if (validateAmountFormFields()) {
            // hide Amount Show Person panels
            togglePanel('#pnlAmount', false);
            togglePanel('#pnlPerson', true);

            // update progress bar
            toggleProgressIndicator('#btnProgressPerson', true, false);
            toggleProgressIndicator('#btnProgressAmount', true, true);

            // Enable button for Amount Progress Indicator
            $('#btnProgressAmount').removeAttr('disabled');
        }
    } else if (targetPanel === 'pnlPayment') {
        // check if person panel is completed
        if (validatePersonFormFields()) {
            // hide Person Show Payment panels
            togglePanel('#pnlPerson', false);
            togglePanel('#pnlPayment', true);

            // update progress bar
            toggleProgressIndicator('#btnProgressPayment', true, false);
            toggleProgressIndicator('#btnProgressPerson', true, true);

            // Enable button for Person Progress Indicator
            $('#btnProgressPerson').removeAttr('disabled');
        }
    } else if (targetPanel === 'pnlConfirm') {

        let siteKey = null;

        siteKey = $('#hfGoogleCaptchaSiteKey').val();

        if (siteKey) {
            grecaptcha.execute(siteKey, { action: 'ccv_transaction_entry' }).then(function (token) {
                $('#hfGoogleCaptchaToken').val(token);
            });
        }
        
        // check if payment fields are populated
        if (validatePaymentFormFields()) {
            populateConfirmFields();

            // hide Payment show Confirm panels
            togglePanel('#pnlPayment', false);
            togglePanel('#pnlConfirm', true);

            //Attach anti Autobot validation.
            megatron.attack();

            // update progress bar
            toggleProgressIndicator('#btnProgressConfirm', true, false);
            toggleProgressIndicator('#btnProgressPayment', true, true);

            // Enable button for Payment Progress Indicator
            $('#btnProgressPayment').removeAttr('disabled');
        }
    }
}

// Back button onClick
btnBack_OnClick = function (targetPanel)
{
    clearErrorFormatting();

    if (targetPanel === 'pnlAmount') {
        // hide Person show Amount panels
        togglePanel('#pnlPerson', false);
        togglePanel('#pnlAmount', true);

        // update progress bar
        toggleProgressIndicator('#btnProgressPerson', false, false);
        toggleProgressIndicator('#btnProgressAmount', true, false);

        // Disable amount progress indicator button
        $('#btnProgressAmount').attr('disabled', 'disabled');
    } else if (targetPanel === 'pnlPerson') {
        // hide Payment show Person panels
        togglePanel('#pnlPayment', false);
        togglePanel('#pnlPerson', true);

        // update progress bar
        toggleProgressIndicator('#btnProgressPayment', false, false);
        toggleProgressIndicator('#btnProgressPerson', true, false);

        // Disable person progress indicator button
        $('#btnProgressPerson').attr('disabled', 'disabled');
    } else if (targetPanel === 'pnlPayment') {
        // hide Confirm show Payment panels
        togglePanel('#pnlConfirm', false);
        togglePanel('#pnlPayment', true);

        // update progress bar
        toggleProgressIndicator('#btnProgressConfirm', false, false);
        toggleProgressIndicator('#btnProgressPayment', true, false);

        // Disable payment progress indicator button
        $('#btnProgressPayment').attr('disabled', 'disabled');
    }
}

// Progress Indicator Button Click
btnProgress_OnClick = function (targetPanel){
    if (targetPanel === 'pnlAmount') {
        // show Amount panel / hide others
        togglePanel('#pnlConfirm', false);
        togglePanel('#pnlPayment', false);
        togglePanel('#pnlPerson', false);
        togglePanel('#pnlAmount', true);

        // set progress indicators
        toggleProgressIndicator('#btnProgressConfirm', false, false);
        toggleProgressIndicator('#btnProgressPayment', false, false);
        toggleProgressIndicator('#btnProgressPerson', false, false);
        toggleProgressIndicator('#btnProgressAmount', true, false);

        // enable / disable progress buttons
        $('#btnProgressAmount').attr('disabled', 'disabled');
        $('#btnProgressPerson').attr('disabled', 'disabled');
        $('#btnProgressPayment').attr('disabled', 'disabled');
    } else if (targetPanel === 'pnlPerson') {
        // show Person panel / hide others
        togglePanel('#pnlAmount', false);
        togglePanel('#pnlConfirm', false);
        togglePanel('#pnlPayment', false);
        togglePanel('#pnlPerson', true);

        // set progress indicators
        toggleProgressIndicator('#btnProgressAmount', true, true);
        toggleProgressIndicator('#btnProgressConfirm', false, false);
        toggleProgressIndicator('#btnProgressPayment', false, false);
        toggleProgressIndicator('#btnProgressPerson', true, false);

        // enable / disable progress buttons
        $('#btnProgressAmount').removeAttr('disabled');;
        $('#btnProgressPerson').attr('disabled', 'disabled');
        $('#btnProgressPayment').attr('disabled', 'disabled');
    } else if (targetPanel === 'pnlPayment') {
        // show Payment panel / hide others
        togglePanel('#pnlPerson', false);
        togglePanel('#pnlAmount', false);
        togglePanel('#pnlConfirm', false);
        togglePanel('#pnlPayment', true);

        // set progress indicators
        toggleProgressIndicator('#btnProgressPerson', true, true);
        toggleProgressIndicator('#btnProgressAmount', true, true);
        toggleProgressIndicator('#btnProgressConfirm', false, false);
        toggleProgressIndicator('#btnProgressPayment', true, false);

        // enable / disable progress buttons
        $('#btnProgressAmount').removeAttr('disabled');;
        $('#btnProgressPerson').removeAttr('disabled');;
        $('#btnProgressPayment').attr('disabled', 'disabled');
    }
}

// Credit Card payment type on onClick
btnCreditCard_OnClick = function () {
    clearErrorFormatting();

    // update payment type hidden field
    $('#hfPaymentType').attr('value', 'CC');

    // hide Bank Account or Saved Payment panel show Credit Card panel and update button selected state
    togglePanel('#pnlBankAccount', false);
    togglePanel('#pnlSavedPayment', false);
    togglePanel('#pnlCreditCard', true);

    toggleButtonSelectedState('#btnBankAccount', false);
    toggleButtonSelectedState('#btnSavedPayment', false);
    toggleButtonSelectedState('#btnCreditCard', true);
}

// ACH payment type onClick
btnBankAccount_OnClick = function () {
    clearErrorFormatting();

    // update payment type hidden field
    $('#hfPaymentType').attr('value','ACH');

    // hide Credit Card or Saved Payment panel show Bank Account panel and update button class
    togglePanel('#pnlCreditCard', false);
    togglePanel('#pnlSavedPayment', false);
    togglePanel('#pnlBankAccount', true);

    toggleButtonSelectedState('#btnCreditCard', false);
    toggleButtonSelectedState('#btnSavedPayment', false);
    toggleButtonSelectedState('#btnBankAccount', true);
}

// Saved Payment type onClick
btnSavedPayment_OnClick = function () {
    clearErrorFormatting();

    // update payment type hidden field
    $('#hfPaymentType').attr('value','REF');

    // hide Credit Card or Bank Account panel show Saved Payment panel and update button class
    togglePanel('#pnlCreditCard', false);
    togglePanel('#pnlBankAccount', false);
    togglePanel('#pnlSavedPayment', true);

    toggleButtonSelectedState('#btnCreditCard', false);
    toggleButtonSelectedState('#btnBankAccount', false);
    toggleButtonSelectedState('#btnSavedPayment', true);
}


populateConfirmFields = function () {
    // populate confirm fields
    if ($('#tglScheduledTransaction').is(':checked')) {
        $('#confirmGiftMessage').html('You are giving a ' + $('#ddlScheduleFrequency').find(':selected').text().toLowerCase() + ' gift of');
        $('#confirmScheduleStartMessage').html('starting on ' + $('#dpScheduledTransactionStartDate').val() + ' using');
    } else {
        $('#confirmGiftMessage').html('You are giving a gift today of');
        $('#confirmScheduleStartMessage').html('using payment account');
    }

    $('#confirmGiftAmount').html($('#nbAmount').val());

    if ($('#pnlCreditCard').hasClass('hidden') && $('#pnlSavedPayment').hasClass('hidden')) {
        // populate bank account info
        $('#accountType').html('Bank Account');
        $('#personName').html($('#tbFirstName').val() + ' ' + $('#tbLastName').val());
        $('#confirmAccountNumber').html($('#nbAccountNumber').val());
        $('#savedAccountName').html($('#rblAccountType input[type=radio]:checked').val());

    } else if ($('#pnlCreditCard').hasClass('hidden') && $('#pnlBankAccount').hasClass('hidden')) {
        // create name and account number
        var savedAccount = $('#ddlSavedPaymentAccounts').find(':selected').text().replace('Use ', '').split('(');
        var savedAccountNumber = savedAccount[1].replace(')', '');

        // populate saved account info
        $('#accountType').html('Saved Account');
        $('#personName').html($('#tbFirstName').val() + ' ' + $('#tbLastName').val());
        $('#confirmAccountNumber').html('**** **** **** ' + savedAccountNumber.substr(savedAccountNumber.length - 4));
        $('#savedAccountName').html(savedAccount[0]);
    } else {
        // populate credit card account info
        // credit card regex patterns
        var visaRegEx = /^(?:4[0-9]{12}(?:[0-9]{3})?)$/;
        var mastercardRegEx = /^(?:5[1-5][0-9]{14})$/;
        var amexpRegEx = /^(?:3[47][0-9]{13})$/;
        var discovRegEx = /^(?:6(?:011|5[0-9][0-9])[0-9]{12})$/;
        
        // Populate Credit Card type
        if (visaRegEx.test($('#nbCreditCard').val().replace(/[^0-9]/g, ''))) {
            $('#accountType').html('Visa');
        } else if (mastercardRegEx.test($('#nbCreditCard').val().replace(/[^0-9]/g, ''))) {
            $('#accountType').html('Master Card');
        } else if (amexpRegEx.test($('#nbCreditCard').val().replace(/[^0-9]/g, ''))) {
            $('#accountType').html('American Express');
        } else if (discovRegEx.test($('#nbCreditCard').val().replace(/[^0-9]/g, ''))) {
            $('#accountType').html('Discover');
        }

        // populate name
        $('#personName').html($('#tbName').val());

        // populate credit card and mask number
        $('#confirmAccountNumber').html('**** **** **** ' + $('#nbCreditCard').val().substr($('#nbCreditCard').length - 5));
    }
}

//
// Validation Methods
//

// Verify Amount Fields are populated
validateAmountFormFields = function () {
    // get amount and selected fund value
    var amount = $('#nbAmount').val();
    var fund = $('#ddlAccounts').find(':selected').val();
    var minDonation = parseFloat($('#hfMinDonation').val());

    // check if schedule input is toggled
    var isScheduledTransaction = false;
    var scheduledTransactionReady = false;
    if ($('#tglScheduledTransaction').is(':checked')) {
        isScheduledTransaction = true;
        // check if its fields have values
        if ($('#ddlScheduleFrequency').find(':selected').val() && $('#ddlScheduleFrequency').find(':selected').val() !== '-1' && /^02\/(?:[01]\d|2\d)\/(?:0[048]|[13579][26]|[2468][048])|(?:0[13578]|10|12)\/(?:[0-2]\d|3[01])\/\d{2}|(?:0[469]|11)\/(?:[0-2]\d|30)\/\d{2}|02\/(?:[0-1]\d|2[0-8])\/\d{2}$/.test($('#dpScheduledTransactionStartDate').val())) {
            scheduledTransactionReady = true;
        } 
    } else {
        // not a scheduled transaction, ready to proceed
        scheduledTransactionReady = true;
    }

    if ((amount && amount !== '$') && (fund && fund !== '-1') && (parseFloat(amount.replace('$', '')) >= minDonation) && scheduledTransactionReady === true && $('.has-error').length === 0) {
        $('#nbHTMLMessage').addClass('hidden');
        return true;
    } else {

        let errorMessage = "Please correct errors and try again.";

        // highlight fields not ready
        if (!amount || amount === '$') {
            $('#nbAmount').parents('div.amount-wrapper').addClass('has-error');
            errorMessage += "<br />You must enter a valid amount greater than $10.00";
        }

        if (parseFloat(amount.replace('$', '')) < minDonation) {
            $('#nbAmount').parents('div.amount-wrapper').addClass('has-error');
            errorMessage += "<br />You must enter a valid amount greater than $10.00";
        }

        if ((!fund || fund === '-1')) {
            $('#ddlAccounts').parents('div.accounts-wrapper').addClass('has-error');
        }

        if (isScheduledTransaction === true && scheduledTransactionReady === false) {
            if (!$('#ddlScheduleFrequency').find(':selected').val() || $('#ddlScheduleFrequency').find(':selected').val() === '-1') {
                $('#ddlScheduleFrequency').parents('div.schedule-transaction-wrapper').addClass('has-error');
            }

            if (!/^02\/(?:[01]\d|2\d)\/(?:0[048]|[13579][26]|[2468][048])|(?:0[13578]|10|12)\/(?:[0-2]\d|3[01])\/\d{2}|(?:0[469]|11)\/(?:[0-2]\d|30)\/\d{2}|02\/(?:[0-1]\d|2[0-8])\/\d{2}$/.test($('#dpScheduledTransactionStartDate').val())) {
                $('#dpScheduledTransactionStartDate').parents('div.schedule-date-wrapper').addClass('has-error');
            }
        }

        displayMessage(errorMessage, 'danger');
        return false;
    }

    return false;
}

// Validate if person fields are populated
validatePersonFormFields = function () {
    var firstName = $('#tbFirstName').val();
    var lastName = $('#tbLastName').val();
    var email = $('#tbEmail').val();

    if (firstName && lastName && email && $('.has-error').length === 0) {
        $('#nbHTMLMessage').addClass('hidden');
        return true;
    } else {
        // highlight missing fields
        if (!firstName) {
            $('#tbFirstName').parents('div.form-group').addClass('has-error');
        }
        if (!lastName) {
            $('#tbLastName').parents('div.form-group').addClass('has-error');
        }
        if (!email) {
            $('#tbEmail').parents('div.form-group').addClass('has-error');
        }

        displayMessage('Please correct errors and try again.', 'danger');
        return false;
    }

    return false;
}

// Validate if payment fields are populated
validatePaymentFormFields = function () {
    if ($('#pnlCreditCard').hasClass('hidden') && $('#pnlSavedPayment').hasClass('hidden')) {
        return validateBankAccountFields();
    } else if ($('#pnlBankAccount').hasClass('hidden') && $('#pnlSavedPayment').hasClass('hidden')) {
        return validateCreditCardFields();
    } else {
        return validateSavedPaymentFields();
    }

    // default false if checks fail to process
    return false;
}

// validate bank account fields
validateBankAccountFields = function () {
    // Check Bank Account Fields
    // build array of elements
    var elementArray = [];

    var routingNumber = $('#nbRoutingNumber');
    var accountNumber = $('#nbAccountNumber');
    elementArray.push(routingNumber);
    elementArray.push(accountNumber);

    // check each element
    elementArray.forEach(checkForValue);

    // check if account type has value
    if ($('#rblAccountType input[type=radio]:checked').val()) {
        $('#rblAccountType').parents('div.form-group').removeClass('has-error');
    } else {
        $('#rblAccountType').parents('div.form-group').addClass('has-error');
    }

    // check page for any errors
    if ($('.has-error').length === 0) {
        // no errorrs
        $('#nbHTMLMessage').addClass('hidden');
        return true;
    } else {
        // page has errors, return false
        displayMessage('Please correct errors and try again.', 'danger');
        return false;
    }
}

// validate credit card fields
validateCreditCardFields = function () {
    // Check Credit Card Account Fields
    var name = $('#tbName').val();
    var creditCard = $('#nbCreditCard').val();
    var expDateMonth = $('#monthDropDownList').find(':selected').val();
    var expDateYear = $('#yearDropDownList_').find(':selected').val();
    var CVV = $('#nbCVV').val();
    var street = $('#tbStreet').val();
    var city = $('#tbCity').val();
    var state = $('#ddlState').find(':selected').val();
    var country = $('#ddlCountry').find(':selected').val();
    var postalCode = $('#nbPostalCode').val();

    if (name && creditCard && expDateMonth && expDateYear && CVV && street && city && state && country && postalCode && $('.has-error').length === 0) {
        $('#nbHTMLMessage').addClass('hidden');
        return true;
    } else {
        // highlight fields not ready
        if (!name) {
           $('#tbName').parents('div.form-group').addClass('has-error');
        }

        if (!creditCard) {
            $('#nbCreditCard').parents('div.creditcard-wrapper').addClass('has-error');
        }

        if (!expDateMonth) {
            $('#monthDropDownList').parents('div.form-group').addClass('has-error');
        }

        if (!expDateYear) {
            $('#yearDropDownList_').parents('div.form-group').addClass('has-error');
        }

        if (!CVV) {
            $('#nbCVV').parents('div.form-group').addClass('has-error');
        }

        if (!street) {
            $('#tbStreet').parents('div.form-group').addClass('has-error');
        }

        if (!city) {
            $('#tbCity').parents('div.form-group').addClass('has-error');
        }

        if (!state) {
            $('#ddlState').parents('div.form-group').addClass('has-error');
        }

        if (!country) {
            $('#ddlCountry').parents('div.form-group').addClass('has-error');
        }

        if (!postalCode) {
            $('#nbPostalCode').parents('div.form-group').addClass('has-error');
        }

        displayMessage('Please correct errors and try again.', 'danger');
        return false;
    }
}

// validate saved payment fields
validateSavedPaymentFields = function () {
    // get selected value of accouts
    var account = $('#ddlSavedPaymentAccounts').find(':selected').val();
    
    // check if payment account has selected value
    if (account && account !== '-1') {
        $('#ddlSavedPaymentAccounts').parents('div.savedpayment-wrapper').removeClass('has-error');
    } else {
        $('#ddlSavedPaymentAccounts').parents('div.savedpayment-wrapper').addClass('has-error');
    }

    // check page for any errors
    if ($('.has-error').length === 0) {
        // no errorrs
        $('#nbHTMLMessage').addClass('hidden');
        return true;
    } else {
        // page has errors, return false
        displayMessage('Please correct errors and try again.', 'danger');
        return false;
    }
}

// Validate success panel input form
validateSuccessInputForm = function () {
    var ready = false;

    // check if save payment account is toggled
    if ($('#tglSavePaymentAccount').is(':checked')) {
        // check if its fields have values
        if ($('#tbSavePaymentAccountName').val()) {
            ready = true;
        } else {
            return false;
        }
    }

    // check if schedule input is toggled
    if ($('#tglSuccessScheduleTransaction').is(':checked')) {
        // check if its fields have values
      if ($('#ddlSuccessScheduleFrequency').find(':selected').val() && $('#ddlSuccessScheduleFrequency').find(':selected').val() !== '-1') {
            // ready for save
            ready = true;
        } else {
            return false;
        }
    }

    return ready;
}

//
//  Helper Methods
//

// Display message in notification box
displayMessage = function (message, alert) {
    // get notification box
    var nb = $('#nbHTMLMessage');

    // set message
    nb.html(message);

    // show notification box and set alert type
    nb.addClass(getAlertClass(alert));
    nb.removeClass('hidden');
}

// clear any errors and hide notifiation box if shown
clearErrorFormatting = function () {
    $('.has-error').removeClass('has-error');
    $('#nbHTMLMessage').addClass('hidden');
}

// return CSS class for alert type
getAlertClass = function (alert) {
    switch (alert) {
        case 'primary': {
            return 'alert-primary';
        }
        case 'secondary': {
            return 'alert-secondary';
        }
        case 'success': {
            return 'alert-success';
        }
        case 'warning': {
            return 'alert-warning';
        }
        case 'info': {
            return 'alert-info';
        }
        case 'light': {
            return 'alert-light';
        }
        case 'dark': {
            return 'alert-dark';
        }
        default: {
            return 'alert-danger';
        }
    }
}

// toggle panel visibility
togglePanel = function (name, show) {
    if (show === true) {
        $(name).removeClass('hidden');
    } else {
        $(name).addClass('hidden');
    }
}

// toggle progress indicator
toggleProgressIndicator = function (name, active, complete) {
    // set active state
    if (active === true) {
        $(name).addClass('active');
    } else {
        $(name).removeClass('active');
    }

    // set complete state
    if (complete === true) {
        $(name).addClass('complete');
    } else {
        $(name).removeClass('complete');
    }
}

// toggle button selected state
toggleButtonSelectedState = function (name, selected) {
    if (selected === true) {
        $(name).addClass('btn-primary');
    } else {
        $(name).removeClass('btn-primary');
    }
}

// check if element has value and set error state
checkForValue = function (item, index) {
    if (!item.val()) {
        $(item).parents('div.form-group').addClass('has-error');
    } else {
        $(item).parents('div.form-group').removeClass('has-error');
    }
}

// evaluate enable/disable save button on success input form
setInputSaveButtonState = function () {
    // enable / disable save button
    if (validateSuccessInputForm()) {
        // enable save button
        $('#btnSaveSuccessInputForm').removeAttr('disabled');
        $('#btnSaveSuccessInputForm').removeClass('aspNetDisabled');
    } else {
        // disable save button
        $('#btnSaveSuccessInputForm').attr('disabled', 'disabled');
        $('#btnSaveSuccessInputForm').addClass('aspNetDisabled');
    }
}