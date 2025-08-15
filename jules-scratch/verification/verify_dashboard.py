from playwright.sync_api import sync_playwright, expect

def run(playwright):
    browser = playwright.chromium.launch(headless=True)
    context = browser.new_context()
    page = context.new_page()

    # Navigate to the app. The API key for the sample tenant is provided in the URL.
    page.goto("http://localhost:3000/?apiKey=acme_api_key_12345")

    # Wait for the dashboard heading to be visible
    dashboard_heading = page.get_by_role("heading", name="Dashboard")
    expect(dashboard_heading).to_be_visible()

    # Take a screenshot of the dashboard
    page.screenshot(path="jules-scratch/verification/verification.png")

    context.close()
    browser.close()

with sync_playwright() as playwright:
    run(playwright)
