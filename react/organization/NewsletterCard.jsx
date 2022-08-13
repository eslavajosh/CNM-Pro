import React from 'react';

function NewsletterCard() {
    return (
        <div className="card">
            <h4 className="header-title mb-3">Latest Newsletter</h4>
            <img
                src="https://binaries.templates.cdn.office.net/support/templates/en-us/lt22618618_quantized.png"
                className="card-img-top"
                alt="newsletter-card-img"
                height={475}
            />
        </div>
    );
}

export default NewsletterCard;
