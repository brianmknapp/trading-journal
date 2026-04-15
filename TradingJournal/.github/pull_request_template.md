## What Changed

- Briefly summarize the change in 1-3 bullets.
- Include key files/components touched.

## Why

- Describe the problem, goal, or context for this change.
- Link supporting discussion/decision records when relevant.

## Scope

- Type: <!-- feat | fix | docs | refactor | test | ci | chore -->
- Scope: <!-- solution | web | tests | docs | ci | deps -->
- Breaking change: <!-- yes/no -->

## Validation

- [ ] `dotnet build TradingJournal.sln -c Release`
- [ ] `dotnet format TradingJournal.sln --verify-no-changes`
- [ ] Relevant tests pass (list below)

Validation notes:

- <!-- e.g., dotnet test tests/TradingJournal.Web.PlaywrightTests/TradingJournal.Web.PlaywrightTests.csproj -c Release -->

## Deployment & Risk

- Deployment target(s): <!-- test/prod/none -->
- Risk level: <!-- low/medium/high -->
- Rollback plan:
  - <!-- e.g., revert commit / redeploy previous artifact -->

## UI/Behavior Evidence (if applicable)

- [ ] Screenshots attached
- [ ] Video attached
- [ ] N/A

## Linked Work

- Closes #
- Related #

## Reviewer Checklist

- [ ] Scope is clear and appropriately sized
- [ ] Tests and validation are sufficient
- [ ] Docs/config updates included where needed
- [ ] No secrets or sensitive data introduced

