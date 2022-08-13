import React, { useEffect, useState } from 'react';
import { GetOrganizationByCreatedBy } from '../../../services/organizationsService';
import * as stripeService from '../../../services/stripeServices';
import logger from 'sabio-debug';
import PropTypes from 'prop-types';
import './orgdash.css';
import StarsRating from 'react-star-rate';
import BlogCard from './BlogCard';
import NewsletterCard from './NewsletterCard';
import Social from '../analytics/Social';
import SessionsChart from './OrgSessionsChart';
import JobsWidget from './JobsWidget';
import Statistics from './OrgStatistics';
import ViewsChart from './ViewsChart';
import { Row, Col } from 'react-bootstrap';
import toastr from 'toastr';
import Engagement from '../analytics/Engagement';
import TaskWidget from './TaskWidget';

const _logger = logger.extend('OrgDash');

function OrgDashboard(props) {
    const [adminData, setAdminData] = useState({
        name: '',
        logo: '',
        organizationType: '',
        subscriptionName: '',
        subscriptionStart: '',
    });
    const [subInfo, setSubInfo] = useState({
        subscriptionName: '',
        subscriptionStart: '',
    });

    false && _logger('admindata', adminData);

    useEffect(() => {
        GetOrganizationByCreatedBy(props.currentUser.id).then(getOrganizationSuccess).catch(getOrganizationError);

        stripeService.getSubStatusById(props.currentUser.id).then(getSubStatusSuccess).catch(getSubStatusError);
    }, []);

    const getOrganizationSuccess = (response) => {
        _logger('getorganizationsuccess', response);
        const organization = response.data.item;

        setAdminData((prevState) => {
            const newState = { ...prevState };
            newState.name = organization.name;
            newState.logo = organization.logo;
            newState.organizationType = organization.organizationType.name;

            return newState;
        });
    };

    const getSubStatusSuccess = (response) => {
        setSubInfo((prevState) => {
            const newState = { ...prevState };
            newState.subscriptionName = response.item.name;
            newState.subscriptionStart = response.item.subscriptionStartDate.substring(0, 10);

            return newState;
        });
    };

    _logger('SUBSCRIPTION Name UPDATED --->>>', subInfo.subscriptionName);
    _logger('SUBSCRIPTION Date UPDATED --->>>', subInfo.subscriptionStart);

    const getSubStatusError = (error) => {
        _logger('get subscriptoin error', error);
        toastr.error('Subscription info not available');
    };

    const getOrganizationError = (error) => {
        _logger('getorganizationerror', error);
        toastr.error('Org info not available');
    };

    return (
        <React.Fragment>
            <Row>
                <Col>
                    <div className="page-title-box">
                        <div className="page-title-right"></div>
                        <img src={adminData.logo} className="img-org-logo-header" alt="..." />
                        {adminData.name} Dashboard
                        <StarsRating count={5} disabled="false" value={5} classNamePrefix="react-star-rate" />
                    </div>
                </Col>
            </Row>

            <Row>
                <Col xl={3} lg={4}>
                    <Statistics />
                </Col>

                <Col xl={9} lg={8}>
                    <SessionsChart />
                </Col>
            </Row>

            <Row>
                <Col>
                    <TaskWidget />
                </Col>
            </Row>

            <Row>
                <Col xl={4} lg={12}>
                    <ViewsChart />
                </Col>
                <Col xl={4} lg={6}>
                    <NewsletterCard />
                </Col>
                <Col xl={4} lg={6}>
                    <JobsWidget />
                </Col>
            </Row>

            <Row>
                <Col xl={4} lg={6}>
                    <BlogCard />
                </Col>
                <Col xl={4} lg={6}>
                    <Social />
                </Col>
                <Col xl={4} lg={6}>
                    <Engagement />
                </Col>
            </Row>
        </React.Fragment>
    );
}

OrgDashboard.propTypes = {
    currentUser: PropTypes.shape({
        email: PropTypes.string.isRequired,
        id: PropTypes.number.isRequired,
        isLoggedIn: PropTypes.bool.isRequired,
        roles: PropTypes.arrayOf(PropTypes.string).isRequired,
    }),
};

export default OrgDashboard;
