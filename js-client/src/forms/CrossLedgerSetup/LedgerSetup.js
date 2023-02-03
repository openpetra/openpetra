// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//	   Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//	   Christopher Jäkel
//
// Copyright 2017-2018 by TBits.net
// Copyright 2019-2023 by SolidCharity.com
//
// This file is part of OpenPetra.
//
// OpenPetra is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.  If not, see <http://www.gnu.org/licenses/>.
//

var last_requested_data = {};

$('document').ready(function () {
	display_list();
});

function display_list() {
	api.post('serverMFinance.asmx/TGLSetupWebConnector_GetAvailableLedgers', {}).then(function (data) {
		data = JSON.parse(data.data.d);
		// on reload, clear content
		$('#browse_container').html('');
		last_requested_data = data.result;
		if (data.result.length == 0) {
			open_new();
		} else {
			for (item of data.result) {
				format_item(item);
			}
		}
	})
}

function format_item(item) {
	let row = format_tpl($("[phantom] .tpl_row").clone(), item);
	let view = format_tpl($("[phantom] .tpl_view").clone(), item);
	row.find('.collapse_col').append(view);
	$('#browse_container').append(row);
}

function open_detail(obj) {
	obj = $(obj);
	if (obj.find('.collapse').is(':visible') ) {
		$('.tpl_row .collapse').collapse('hide');
		return;
	}
	$('.tpl_row .collapse').collapse('hide');
	obj.find('.collapse').collapse('show');
}

function open_edit(sub_id) {
	if (!allow_modal()) {return}
	let z = null;
	for (sub of last_requested_data) {
		if (sub.a_ledger_number_i == sub_id) {
			z = sub;
			break;
		}
	}

	let p = {};
	api.post('serverMFinance.asmx/TGLSetupWebConnector_GetCountryCodes', p).then(function (data) {
		data = JSON.parse(data.data.d);

		let m = $('[phantom] .tpl_edit').clone();
		m = load_countries(data.AResultTable, z.a_country_code_c, m);

		p['ALedgerNumber'] = window.localStorage.getItem('current_ledger');
		api.post('serverMFinance.asmx/TGLSetupWebConnector_GetLedgerSystemDefaults', p).then(function (data) {
			data = JSON.parse(data.data.d);

			data["a_sepa_creditor_name_c"] = data["ASepaCreditorName"];
			data["a_sepa_creditor_iban_c"] = data["ASepaCreditorIban"];
			data["a_sepa_creditor_bic_c"] = data["ASepaCreditorBic"];
			data["a_sepa_creditor_scheme_id_c"] = data["ASepaCreditorSchemeId"];
			data["a_receipt_email_publication_code_c"] = data["AReceiptEmailPublicationCode"];
			m = format_tpl(m, data);

			var f = format_tpl( m, z);
			$('#modal_space').html(f);
			$('#modal_space .modal').modal('show');
		})

	})
}

function open_new() {
	if (!allow_modal()) {return}

	let p = {};
	api.post('serverMFinance.asmx/TGLSetupWebConnector_GetCountryCodes', p).then(function (data) {
		data = JSON.parse(data.data.d);

		let m = $('[phantom] .tpl_new').clone();

		let country_code = currentLng();
		if (country_code.includes('-')) {
			country_code = country_code.substring(country_code.indexOf('-') + 1);
		}
		m = load_countries(data.AResultTable, country_code.toUpperCase(), m);

		let p = {};
		api.post('serverMFinance.asmx/TGLSetupWebConnector_GetCurrencyCodes', p).then(function (data) {
			data = JSON.parse(data.data.d);
			m = load_currencies(data.AResultTable, "EUR", m);

			$('#modal_space').html(m);
			var today = new Date();
			var yyyy = today.getFullYear();
			var today = yyyy+"-01-01";
			$('#modal_space .modal').modal('show');
			$('#modal_space .modal #ACalendarStartDate').val(today);
			$('#modal_space .modal #ANewLedgerNumber').val(10);
			$('#modal_space .modal #ALedgerName').val(i18next.t('LedgerSetup.example_name'));
		});
	});
}

function save_new() {
		if (!allow_modal()) {return}
		let se = $('#modal_space .modal').modal('show');
		let d = extract_data(se);

		let request = translate_to_server(d);
		request['AIntlCurrency'] = 'EUR';
		api.post("serverMFinance.asmx/TGLSetupWebConnector_CreateNewLedger", request).then(
			function (result) {
				parsed = JSON.parse(result.data.d);
				if (parsed.result == true) {
					display_message(i18next.t('LedgerSetup.confirm_create'), 'success');
					se.modal('hide');
					// reload permissions so that the current user has access to the new ledger
					api.post("serverMSysMan.asmx/TUserManagerWebConnector_ReloadUserInfo", request);
					// from lib/navigation.js, call LoadAvailableLedgerDropDown
					LoadAvailableLedgerDropDown();
					display_list();
				} else {
					display_error(parsed.AVerificationResult, 'LedgerSetup', 'fail');
				}
			}
		)

}

function save_entry(update) {
	let raw = $(update).closest('.modal');
	let request = translate_to_server(extract_data(raw));

	request['action'] = 'update';

	api.post("serverMFinance.asmx/TGLSetupWebConnector_MaintainLedger", request).then(
		function (result) {
			parsed = JSON.parse(result.data.d);
			if (parsed.result == true) {
				CloseModal(raw);
				display_message(i18next.t('LedgerSetup.confirm_edit'), 'success');
				display_list();
			}
		}
	)
}

function delete_entry(d) {
	let raw = $(d).closest('.modal');
	let request = translate_to_server(extract_data(raw));

	request['action'] = 'delete';

	let s = confirm( i18next.t('LedgerSetup.ask_delete') );
	if (!s) {return}

	api.post("serverMFinance.asmx/TGLSetupWebConnector_MaintainLedger", request).then(
		function (result) {
			parsed = JSON.parse(result.data.d);
			if (parsed.result == true) {
				CloseModal(raw);
				display_message(i18next.t('LedgerSetup.confirm_delete'), 'success');
				display_list();
			}
		}
	);

}

function load_countries(all_countries, selected_country, obj) {

	if (selected_country == null) selected_country="99";
	for (country of all_countries) {
		selected = (selected_country == country.p_country_code_c)?" selected":"";
		let y = $('<option value="'+country.p_country_code_c+'"' + selected + '>'+country.p_country_code_c + " " + country.p_country_name_c + '</option>');
		obj.find('#CountryCode').append(y);
	}
	return obj;
}

function load_currencies(all, selected_currency, obj) {

	if (selected_currency == null) selected_currency="EUR";
	for (currency of all) {
		let selected = (selected_currency == currency.a_currency_code_c)?" selected":"";
		let y = $('<option value="'+currency.a_currency_code_c+'"' + selected + '>'+currency.a_currency_code_c + " " + currency.a_currency_name_c + '</option>');
		obj.find('#BaseCurrency').append(y);
	}
	return obj;
}
