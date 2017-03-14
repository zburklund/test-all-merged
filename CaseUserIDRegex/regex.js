/*
Case ID Regular Expression 
4/1/2015 bdm4 
v0.1

CaseUserIDs can be

abc
abc1
abc12
abc123
abc1234
But not
abc0 
abc12345
*/

/*
Case ID Regular Expression 
4/1/2015 bdm4 
v0.1

CaseUserIDs can be

abc
abc1
abc12
abc123
abc1234

But not
abc0 
abc12345
*/

$(function () {
    var caseidregex = /^[a-zA-Z]{3}([1-9]\d{0,3})?$/;
    var abc = "abc",    
        abc1 = "abc1",
        abc12 = "abc12",
        abc123 = "abc123",
        abc1234 = "abc1234",
        abc0 = "abc0",
        abc12345 = "abc12345";

    
	console.log("Testing '" + abc + "' : " + caseidregex.test(abc));
	console.log("Testing '" + abc1 + "' : " + caseidregex.test(abc1));
	console.log("Testing '" + abc12 + "' : " + caseidregex.test(abc12));
	console.log("Testing '" + abc123 + "' : " + caseidregex.test(abc123));
	console.log("Testing '" + abc1234 + "' : " + caseidregex.test(abc1234));
	console.log("Testing '" + abc0 + "' : " + caseidregex.test(abc0));
	console.log("Testing '" + abc12345 + "' : " + caseidregex.test(abc12345));
})



