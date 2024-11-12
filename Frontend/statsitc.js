$(document).ready(function () {
    let ratingChart = null; // Initialize chart variables
    let ratingOverTimeChart = null;
    const ctx = document.getElementById('categoryRateChart').getContext('2d');

    // Fetch category list and their ratings to display in the radar chart
    function fetchCategoryRates() {
        $.ajax({
            url: 'https://localhost:7229/api/Category',
            type: 'GET',
            success: function (categories) {
                const categoryNames = [];
                const categoryRatings = [];
                let completedRequests = 0;

                // Fetch each category's rate
                categories.forEach((category, index) => {
                    $.ajax({
                        url: `https://localhost:7229/api/Review/GetRate/categoryId?categoryId=${category.categoryId}`,
                        type: 'GET',
                        success: function (response) {
                            categoryNames.push(category.categoryName);
                            categoryRatings.push(response || 0);

                            // Check if all requests are completed
                            completedRequests++;
                            if (completedRequests === categories.length) {
                                // Once all data is fetched, create the chart
                                createRadarChart(ctx, categoryNames, categoryRatings);
                            }
                        },
                        error: function (error) {
                            console.error('Error fetching rate for category:', error);
                        }
                    });
                });
            },
            error: function (error) {
                console.error('Error fetching categories:', error);
            }
        });
    }

    // Create the radar chart with fetched data
    function createRadarChart(ctx, labels, data) {
        new Chart(ctx, {
            type: 'radar',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Average Rating by Category',
                    data: data,
                    backgroundColor: 'rgba(54, 162, 235, 0.2)',
                    borderColor: 'rgba(54, 162, 235, 1)',
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    r: {
                        beginAtZero: true,
                        max: 5
                    }
                }
            }
        });
    }

    // Call the function to fetch category rates and create the chart
    fetchCategoryRates();
    // Fetch the product data from the API
    $.ajax({
        url: 'https://localhost:7229/api/Product',
        method: 'GET',
        success: function (data) {
            const productSelect = $('#productSelect');
            data.forEach(product => {
                const option = $('<option>').val(product.productId).text(product.productName);
                productSelect.append(option);
            });

            // Set initial selection to option 2 (if it exists)
            if (data.length >= 2) {
                productSelect.val(data[0].productId); // Set to the second product
                fetchProductRatingData(data[0].productId); // Fetch data for the initially selected product
            }
        },
        error: function (xhr, status, error) {
            console.error("Error fetching product data: ", error);
        }
    });

    // Event listener for selecting a product from the dropdown
    $('#productSelect').on('change', function () {
        const productId = $(this).val();
        if (productId) {
            fetchProductRatingData(productId);
        }
    });

    function fetchProductRatingData(productId) {
        if (ratingChart) ratingChart.destroy(); // Clear existing chart
        if (ratingOverTimeChart) ratingOverTimeChart.destroy();

        // Fetch data for overall rating (by stars)
        $.ajax({
            url: `https://localhost:7229/api/Review/GetAllRating/productId?productId=${productId}`,
            method: 'GET',
            success: function (data) {
                const ratingData = getRatingData(data);
                const ctx = document.getElementById('ratingChart').getContext('2d');
                ratingChart = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: ['1 Star', '2 Stars', '3 Stars', '4 Stars', '5 Stars'],
                        datasets: [{
                            label: 'Number of Reviews',
                            data: ratingData,
                            backgroundColor: 'rgba(54, 162, 235, 0.2)',
                            borderColor: 'rgba(54, 162, 235, 1)',
                            borderWidth: 1
                        }]
                    },
                    options: {
                        scales: {
                            y: {
                                beginAtZero: true
                            }
                        }
                    }
                });
            },
            error: function (xhr, status, error) {
                console.error("Error fetching rating data: ", error);
            }
        });

        const startDate = new Date('2024-11-01');
        const endDate = new Date('2024-11-09');
        fetchRatingDataForWeek(startDate, endDate, productId);
    }

    function getRatingData(reviews) {
        const ratingCount = [0, 0, 0, 0, 0];
        reviews.forEach(review => {
            if (review.star >= 1 && review.star <= 5) {
                ratingCount[review.star - 1] += 1;
            }
        });
        return ratingCount;
    }

    function fetchRatingDataForWeek(startDate, endDate, productId) {
        const dates = [];
        const avgRatings = [];
        for (let date = new Date(startDate); date <= endDate; date.setDate(date.getDate() + 1)) {
            const formattedDate = date.toLocaleDateString("en-US");
            $.ajax({
                url: `https://localhost:7229/api/Review/Rating/productId/date?productId=${productId}&date=${formattedDate}`,
                method: 'GET',
                success: function (data) {
                    dates.push(formattedDate);
                    avgRatings.push(data);
                    if (dates.length === 9) {
                        const sortedDates = dates.map((date, index) => ({ date, rating: avgRatings[index] }))
                            .sort((a, b) => new Date(a.date) - new Date(b.date));
                        const sortedDatesArray = sortedDates.map(item => item.date);
                        const sortedRatingsArray = sortedDates.map(item => item.rating);
                        createRatingOverTimeChart(sortedDatesArray, sortedRatingsArray);
                    }
                },
                error: function (xhr, status, error) {
                    console.error(`Error fetching data for date ${formattedDate}:`, error);
                }
            });
        }
    }

    function createRatingOverTimeChart(dates, avgRatings) {
        const ctx = document.getElementById('ratingOverTimeChart').getContext('2d');
        ratingOverTimeChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: dates,
                datasets: [{
                    label: 'Average Rating',
                    data: avgRatings,
                    fill: false,
                    borderColor: 'rgba(75, 192, 192, 1)',
                    tension: 0.1
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: false,
                        min: 0,
                        max: 5,
                        ticks: {
                            stepSize: 1
                        }
                    },
                    x: {
                        ticks: {
                            autoSkip: true,
                            maxRotation: 45,
                            minRotation: 45
                        }
                    }
                }
            }
        });
    }
});