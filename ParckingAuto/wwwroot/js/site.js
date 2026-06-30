window.renderStatsCharts = function (mois, coutData, fuelCostData, consoMoisData, consoLabels, consoData) {
    if (typeof Chart === 'undefined') {
        console.error('Chart.js non chargé');
        return;
    }

    var coutCanvas = document.getElementById('coutChart');
    var fuelCostCanvas = document.getElementById('fuelCostChart');
    var consoMoisCanvas = document.getElementById('consoMoisChart');
    var consoCanvas = document.getElementById('consoChart');
    if (!coutCanvas || !consoCanvas) return;

    if (window._coutChart) window._coutChart.destroy();
    if (window._fuelCostChart) window._fuelCostChart.destroy();
    if (window._consoMoisChart) window._consoMoisChart.destroy();
    if (window._consoChart) window._consoChart.destroy();

    window._coutChart = new Chart(coutCanvas.getContext('2d'), {
        type: 'bar',
        data: {
            labels: mois || [],
            datasets: [{ label: 'Coût maintenance (DT)', data: coutData || [], backgroundColor: '#C1121F' }]
        },
        options: { responsive: true, maintainAspectRatio: false }
    });

    if (fuelCostCanvas) {
        window._fuelCostChart = new Chart(fuelCostCanvas.getContext('2d'), {
            type: 'bar',
            data: {
                labels: mois || [],
                datasets: [{ label: 'Coût carburant (DT)', data: fuelCostData || [], backgroundColor: '#780000' }]
            },
            options: { responsive: true, maintainAspectRatio: false }
        });
    }

    if (consoMoisCanvas) {
        window._consoMoisChart = new Chart(consoMoisCanvas.getContext('2d'), {
            type: 'line',
            data: {
                labels: mois || [],
                datasets: [{
                    label: 'Consommation moyenne (L/100km)',
                    data: consoMoisData || [],
                    borderColor: '#669BBC',
                    backgroundColor: 'rgba(102, 155, 188, 0.15)',
                    fill: true,
                    tension: 0.3
                }]
            },
            options: { responsive: true, maintainAspectRatio: false }
        });
    }

    window._consoChart = new Chart(consoCanvas.getContext('2d'), {
        type: 'bar',
        data: {
            labels: consoLabels || [],
            datasets: [{ label: 'Consommation moyenne (L/100km)', data: consoData || [], backgroundColor: '#669BBC' }]
        },
        options: { responsive: true, maintainAspectRatio: false }
    });
};

window.renderCharts = function (mois, litresData, enMission, auParking, maintenanceCostData) {
    if (typeof Chart === 'undefined') {
        console.error('Chart.js non chargé');
        return;
    }

    var fuelCanvas = document.getElementById('fuelChart');
    var vehiculesCanvas = document.getElementById('vehiculesChart');
    var maintenanceCanvas = document.getElementById('maintenanceChart');
    if (!fuelCanvas || !vehiculesCanvas) return;

    if (window._fuelChart) window._fuelChart.destroy();
    if (window._vehiculesChart) window._vehiculesChart.destroy();
    if (window._maintenanceChart) window._maintenanceChart.destroy();

    window._fuelChart = new Chart(fuelCanvas.getContext('2d'), {
        type: 'line',
        data: {
            labels: mois || [],
            datasets: [{
                label: 'Litres consommés',
                data: litresData || [],
                borderColor: '#669BBC',
                backgroundColor: 'rgba(102, 155, 188, 0.15)',
                fill: true,
                tension: 0.3
            }]
        },
        options: { responsive: true, maintainAspectRatio: false }
    });

    window._vehiculesChart = new Chart(vehiculesCanvas.getContext('2d'), {
        type: 'doughnut',
        data: {
            labels: ['En mission', 'Au parking'],
            datasets: [{
                data: [enMission || 0, auParking || 0],
                backgroundColor: ['#003049', '#669BBC']
            }]
        },
        options: { responsive: true, maintainAspectRatio: false }
    });

    if (maintenanceCanvas) {
        window._maintenanceChart = new Chart(maintenanceCanvas.getContext('2d'), {
            type: 'bar',
            data: {
                labels: mois || [],
                datasets: [{
                    label: 'Coût maintenance (DT)',
                    data: maintenanceCostData || [],
                    backgroundColor: '#C1121F'
                }]
            },
            options: { responsive: true, maintainAspectRatio: false }
        });
    }
};

window.authStorage = {
    save: function (token, role, userName) {
        localStorage.setItem('parc_token', token);
        localStorage.setItem('parc_role', role);
        localStorage.setItem('parc_user', userName);
    },
    clear: function () {
        localStorage.removeItem('parc_token');
        localStorage.removeItem('parc_role');
        localStorage.removeItem('parc_user');
    },
    load: function () {
        return {
            token: localStorage.getItem('parc_token') || '',
            role: localStorage.getItem('parc_role') || '',
            userName: localStorage.getItem('parc_user') || ''
        };
    }
};
