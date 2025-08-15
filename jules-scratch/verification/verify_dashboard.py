from playwright.sync_api import sync_playwright, expect
import sys

def run(playwright):
    browser = playwright.chromium.launch(headless=True)
    context = browser.new_context()
    page = context.new_page()

    # Listen for console errors
    def handle_console(msg):
        if msg.type == "error":
            print(f"PAGE CONSOLE ERROR: {msg.text}", file=sys.stderr)

    page.on("console", handle_console)

    # Navigate to the app
    page.goto("http://localhost:3000/?apiKey=acme_api_key_12345")

    try:
        # Wait for the dashboard heading to be visible
        dashboard_heading = page.get_by_role("heading", name="Dashboard")
        expect(dashboard_heading).to_be_visible(timeout=10000)

        # Take a screenshot on success
        page.screenshot(path="jules-scratch/verification/verification.png")
        print("Successfully took screenshot.")

    except Exception as e:
        print(f"Test failed: {e}", file=sys.stderr)
        # Take a screenshot on failure
        page.screenshot(path="jules-scratch/verification/verification-error.png")
        print("Took error screenshot.")

    finally:
        context.close()
        browser.close()

with sync_playwright() as playwright:
    run(playwright)
