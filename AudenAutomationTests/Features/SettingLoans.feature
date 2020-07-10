Feature: SettingLoans
	In order to make sure I can get required amount of loan
	As a user
	I should be able to select required loan amount

Background:
	Given slider by default is set to Min loan value

Scenario: Set loan amount using slider
	When user set loan amount using slider to "differentValueToTheDefault"
	Then the loan amount should matches the amount set on slider

Scenario: First Repayment Day Option as Friday when user selects weekend as payment day
	When user set loan amount using slider to "differentValueToTheDefault"
	And select payment date as Sunday date
	Then Friday date is suggested as First repayment date

Scenario: Set loan amount using slider should not effect the Min-Max load amounts
	When user set loan amount using slider to "Max"
	Then 'Min' loan amount value should be '200' on the slider
	And 'Max' loan amount value should be '500' on the slider