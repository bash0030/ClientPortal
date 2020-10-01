var questionDict = {
    'FullName': 1,
    'Aliases': 2, 
    'CitizenshipImmigrationStatus': 3,
    'DateOfBirth': 4,
    'IndigenousStatus': 5,
    'Gender': 6,
    'MedicAlert': 7,
    'Disability': 8,
    'VeteranStatus': 9,
    'CountryOfBirth': 10,
    'Review': 11
};

function toggleSubmitBtn(canSubmit) {
    if (canSubmit) {
        $('#submitBtn').removeAttr('disabled');
    } else {
        $('#submitBtn').attr('disabled', 'disabled');
    }
}

function toggleQuestion(query, questionNumber, check = 'true') {
    var condition = $(query).val() === check;

    if (condition) {
        $(`#_q${questionNumber}`).show();
    } else {
        $(`#_q${questionNumber}`).hide();
    }
}

function getLastToken(url) {
    var blocks = url.split('?')[0].split('/');
    return blocks.pop();
}

function getReferrer() {
    var url = document.referrer;
    var referrer = getLastToken(url);

    return referrer;
}

function setQuestionPosition(currentQuestion) {
    var referrer = getReferrer();

    if (questionDict[currentQuestion] < questionDict[referrer]) {
        $('#question_form').css({ left: '-100vw', display: 'block' });
    } else {
        $('#question_form').css({ left: '100vw', display: 'block' });
    }

    $('#question_form').animate({
        left: '0vw'
    }, 500);  
}

function setActiveLink() {
    var url = window.location.href;

    var linkName = getLastToken(url);

    var navLink = (linkName === '') ? $(`.nav.navbar-nav > li > a:contains('Home')`) : $(`.nav.navbar-nav > li > a:contains('${linkName}')`);

    navLink.toggleClass('active');
}