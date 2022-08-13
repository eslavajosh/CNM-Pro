// @flow
import React from 'react';
import { Card } from 'react-bootstrap';
import { Link } from 'react-router-dom';

// images
import '../../../assets/scss/custom/plugins/icons/_materialdesignicons.scss';
import '../../dashboard/organization/orgdash.css';

const CampaignWidget = () => {
    return (
        <React.Fragment>
            <Link to="/jobs">
                <Card className="cta-box bg-primary text-white">
                    <Card.Body>
                        <div className="d-flex align-items-start align-items-center">
                            <div className="w-100 overflow-hidden">
                                <h2 className="mt-0">
                                    <i className="mdi mdi-bullhorn-outline"></i>
                                </h2>
                                <h3 className="m-0 fw-normal cta-box-title">
                                    View <b>Available</b> Jobs <i className="mdi mdi-arrow-right"></i>
                                </h3>
                            </div>
                        </div>
                    </Card.Body>
                </Card>
            </Link>

            <Link to="/admin/organization/job/add">
                <Card className="cta-box bg-primary text-white">
                    <Card.Body>
                        <div className="d-flex align-items-start align-items-center">
                            <div className="w-100 overflow-hidden">
                                <h2 className="mt-0">
                                    <i className="mdi mdi-bullhorn-outline"></i>
                                </h2>
                                <h3 className="m-0 fw-normal cta-box-title">
                                    Create a <b>Job</b> Listing <i className="mdi mdi-arrow-right"></i>
                                </h3>
                            </div>
                        </div>
                    </Card.Body>
                </Card>
            </Link>
        </React.Fragment>
    );
};

export default CampaignWidget;
