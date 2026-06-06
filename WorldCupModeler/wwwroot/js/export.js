// Bracket screenshot export using html2canvas (bundled in lib/html2canvas).
window.worldCupExport = {
    // Rasterize a flag-icons SVG to a PNG data URL at an exact pixel size.
    // flag-icons SVGs only carry a viewBox (no intrinsic width/height), so
    // html2canvas draws them at a huge size and clips to the box. Drawing them
    // onto a canvas with an explicit destination size scales them correctly, and
    // the resulting PNG (which has real pixel dimensions) rasterizes perfectly.
    _rasterizeFlag(code, w, h) {
        return new Promise((resolve) => {
            const img = new Image();
            img.onload = () => {
                try {
                    const c = document.createElement("canvas");
                    c.width = w;
                    c.height = h;
                    const cx = c.getContext("2d");
                    cx.drawImage(img, 0, 0, w, h);
                    resolve(c.toDataURL("image/png"));
                } catch (e) {
                    resolve(null);
                }
            };
            img.onerror = () => resolve(null);
            img.src = "lib/flag-icons/flags/4x3/" + code + ".svg";
        });
    },

    async bracketPng(captureSelector, scrollSelector, fileName) {
        const node = document.querySelector(captureSelector);
        if (!node) {
            throw new Error("Capture element not found: " + captureSelector);
        }
        if (typeof html2canvas !== "function") {
            throw new Error("html2canvas is not loaded.");
        }

        const FLAG_W = 30;
        const FLAG_H = 22;
        const SS = 2; // supersample the rasterized flag for crispness

        // The bracket lives inside a horizontally-scrolling container that clips
        // its content. Temporarily let it expand to its full width so the whole
        // bracket is captured, then restore the original styles afterwards.
        const scroller = scrollSelector ? document.querySelector(scrollSelector) : null;
        const savedScroll = scroller
            ? { overflow: scroller.style.overflow, width: scroller.style.width }
            : null;
        if (scroller) {
            scroller.style.overflow = "visible";
            scroller.style.width = "max-content";
        }

        // Collect flag spans and their ISO codes.
        const spanList = [];
        const codes = new Set();
        for (const span of node.querySelectorAll(".flag")) {
            const code = (Array.from(span.classList).find((c) => c.startsWith("fi-")) || "").slice(3);
            if (!code) continue;
            spanList.push({ span, code });
            codes.add(code);
        }

        // Pre-rasterize each unique flag once.
        const pngByCode = {};
        await Promise.all(
            Array.from(codes).map(async (code) => {
                pngByCode[code] = await this._rasterizeFlag(code, FLAG_W * SS, FLAG_H * SS);
            })
        );

        // Swap each flag span for a PNG <img> that html2canvas renders correctly.
        const swaps = [];
        for (const { span, code } of spanList) {
            const png = pngByCode[code];
            if (!png) continue;
            const img = document.createElement("img");
            img.src = png;
            img.width = FLAG_W;
            img.height = FLAG_H;
            img.style.cssText =
                "width:" + FLAG_W + "px;height:" + FLAG_H + "px;border-radius:3px;" +
                "vertical-align:middle;display:inline-block;" +
                "box-shadow:0 0 0 1px rgba(0,0,0,.12);";
            span.style.display = "none";
            span.parentNode.insertBefore(img, span);
            swaps.push({ span, img });
        }

        // Make sure every replacement image is decoded before rasterizing.
        await Promise.all(swaps.map((s) => (s.img.decode ? s.img.decode().catch(() => {}) : Promise.resolve())));

        try {
            const canvas = await html2canvas(node, {
                backgroundColor: "#f4f6fb",
                scale: Math.min(2, window.devicePixelRatio || 1) * 1.5,
                useCORS: true,
                logging: false,
                windowWidth: node.scrollWidth + 64,
            });

            const dataUrl = canvas.toDataURL("image/png");
            const link = document.createElement("a");
            link.href = dataUrl;
            link.download = fileName || "world-cup-2026-bracket.png";
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            return true;
        } finally {
            if (scroller && savedScroll) {
                scroller.style.overflow = savedScroll.overflow;
                scroller.style.width = savedScroll.width;
            }
            for (const s of swaps) {
                s.img.remove();
                s.span.style.display = "";
            }
        }
    }
};
