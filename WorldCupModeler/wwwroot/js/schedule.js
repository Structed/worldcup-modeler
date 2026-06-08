// Localizes match kickoff times to the user's browser timezone + locale.
// Each match time is rendered as <span class="match-when" data-utc="<ISO-8601 UTC>">…</span>;
// this fills the element text with a locale-formatted local date/time.
window.worldCupSchedule = {
    formatLocalTimes(root) {
        const scope = (root && root.querySelectorAll) ? root : document;
        const els = scope.querySelectorAll(".match-when[data-utc]");
        for (const el of els) {
            const iso = el.getAttribute("data-utc");
            if (!iso) continue;
            const d = new Date(iso);
            if (isNaN(d.getTime())) continue;
            try {
                el.textContent = d.toLocaleString(undefined, {
                    weekday: "short",
                    day: "numeric",
                    month: "short",
                    year: "numeric",
                    hour: "2-digit",
                    minute: "2-digit",
                });
            } catch (e) {
                el.textContent = d.toISOString();
            }
        }
    }
};
