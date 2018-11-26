"use strict";

var options = {};
options.ui = {
    showPopover: true,
    showProgressBar: false,
    showVerdicts: true,
    showErrors: true,
    popoverError: function (options) {
        var errors = options.instances.errors,
            errorsTitle = options.i18n.t("errorList"),
            message = "<div>";

        jQuery.each(errors, function (idx, err) {
            message += "<span style='color: #d52929; letter-spacing: 1px'>" + err + "</span>";
        });
        message += "</div>";
        return message;
    }
};
options.rules = {
    activated: {
        wordTwoCharacterClasses: true,
        wordRepetitions: true
    }
};
options.common = {
    minChar: 8,
    debug: true
};
options.ruleScores = {
    notRequiredStrength: -500,
    wordTwoCharacterClasses: 2,
    wordRepetitions: -25
};
options.rules = {
    activated: {
        wordNotEmail: false,
        wordSimilarToUsername: false,
        wordRepetitions: false,
        wordSequences: false,
        wordTwoCharacterClasses: true,
        notRequiredStrength: true
    }
};
options.validationRules = {
    notRequiredStrength: function (options, word, score) {
        return word.match(/(?=^[^\s]{8,16}$)((?=.*?\d)(?=.*?[A-Z])(?=.*?[a-z])|(?=.*?\d)(?=.*?[^\w\d\s])(?=.*?[a-z])|(?=.*?[^\w\d\s])(?=.*?[A-Z])(?=.*?[a-z])|(?=.*?\d)(?=.*?[A-Z])(?=.*?[^\w\d\s]))^.*/) && score;
    }
};

