// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2019-2024 by SolidCharity.com
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

import axios from 'axios';
import i18next from 'i18next';

class ReleaseNotes {

    Ready() {

        let refresh = "?" + Date.now();

        var releasenotes = axios.create({
            baseURL: '/releasenotes/'
        });

        let x = {};
        releasenotes.get(i18next.t('ReleaseNotes.releasenotes_url') + refresh,x)
            .then(function(data) {
            $('#contentReleaseNotes').html(data.data);
            })
            .catch(function (error) {
            // default to english release notes
            releasenotes.get('releasenotes.html' + refresh,x)
                .then(function(data) {
                $('#contentReleaseNotes').html(data.data);
                });
            })
    }
}

var releasenotes = new ReleaseNotes();

export default releasenotes;