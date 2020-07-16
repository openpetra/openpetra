// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2019-2020 by SolidCharity.com
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

$(document).on('show.bs.modal', '.modal', function (event) {
	var zIndex = 1040 + (10 * $('.modal:visible').length);
	$(this).css('z-index', zIndex);
	setTimeout(function() {
		$('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
	}, 0);
});

$(document).on('hidden.bs.modal', function () {
    $(this).data('bs.modal', null);
});


function ShowModal(id, html) {
  // clear previous instance of this modal
  $('#modal_space #'+id).remove();
  // set the id for this modal
  $(html).attr('id', id);
  // insert the new modal
  $('#modal_space').append(html);
  // get the reference to this new modal
  modal = $('#modal_space #'+id);
  // make sure that we cannot close this modal by clicking outside
  modal.modal({backdrop:"static", keyboard: false});
  // show the modal
  modal.modal('show');
  return modal;
}

function FindModal(id) {
  return $('#modal_space #'+id);
}

function FindMyModal(obj) {
  return $(obj.closest(".modal"))
}

function CloseModal(id) {
  $("#modal_space #" + id).modal('hide');
//  $("#modal_space #" + id).remove();
}
