
var  arabicNumbers  = ["٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩"];
var  defaultNumbers = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"];    // add for future enhancement dynamic row language wise logic
var  numericNumber = [];

function getNumericByLanguage(LanguageName) {

    var numberArray =[];

    switch (LanguageName) {
        case "arabic":
            numberArray = arabicNumbers;
            break;
        case "english":
            numberArray = defaultNumbers;
            break;
        default:
            numberArray = defaultNumbers;
    }
    return  numberArray;
}

function getLanguageName(Language) {

    var LanguageName = "";

    if (Language.toLowerCase().indexOf("arabic") != -1) {
        LanguageName = "arabic";
    }
    else if (Language.toLowerCase().indexOf("english") != -1) {
        LanguageName = "english";
    }
    return LanguageName;
}
 
function englishToNumeric(number, LanguageName) {

    numericNumber = getNumericByLanguage(LanguageName);

    var outputNumber = []; var chars = [];
    var convertNumeric = "";

    for (var k = 0; k < number.length; k++) {
        outputNumber.push(number.charAt(k));
    }

    for (var i = 0; i < outputNumber.length; i++) {
        chars[i] = numericNumber[outputNumber[i]];
        convertNumeric = chars.join("");
    }
    debugger;
    return convertNumeric;
}

function englishToOtherLanguageDate(date, Language) {
    var LanguageName = "";          //var dir = "LTR";
   
    if (Language.toLowerCase()== "arabic") {
        LanguageName = "arabic";    //  dir = "RTL";
    }
    else if (Language.toLowerCase()== "english"){
        LanguageName = "english";
    }
    
    var dateArray = [];
    if (date.indexOf('-') != -1) {
        dateArray = date.split('-');
    }
    else if (date.indexOf('/') != -1) {
        dateArray = date.split('/');
    }
    var dd, mm, yyyy, convertedDate = "";

    yyyy = datePartTranslate(dateArray[dateArray.length - 1], LanguageName);         // 3-1 => dateArray.length[2] ==  yyyy
    dd = datePartTranslate(dateArray[dateArray.length - 2], LanguageName);           // 3-1 => dateArray.length[1] ==  dd
    mm = datePartTranslate(dateArray[dateArray.length - 3], LanguageName);           // 3-3 => dateArray.length[0] ==  mm

    convertedDate = dd + "/" + mm + "/" + yyyy;
    return convertedDate;
}



function datePartTranslate(datePart, LanguageName) {

    numericNumber = getNumericByLanguage(LanguageName);

    var outputNumber = []; var chars = [];
    var convertDatePart = "";

    for (var k = 0; k < datePart.length; k++) {
        outputNumber.push(datePart.charAt(k));
    }

    for (var i = 0; i < outputNumber.length; i++) {
        chars[i] = numericNumber[outputNumber[i]];
        convertDatePart = chars.join("");
    }
    return convertDatePart;
}
