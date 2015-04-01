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

var caseidregex = "/^[a-zA-Z]{3}([1-9]\d{0,3})?$/";

$(function(){
	$("#TestBtn").click(function(){
		var totest = $("#CaseIdTxt").html();
		if(caseidregex.match(totest))
			$("#ResultsTxt").html(totest + "matches the case id pattern");
		else
			$("#ResultsTxt").html(totest + "does not match the case id pattern");		
	});		
})

