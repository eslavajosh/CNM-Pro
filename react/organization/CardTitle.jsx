// @flow
import React from 'react';
import { Dropdown } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import classNames from 'classnames';
import PropTypes from 'prop-types';

function CardTitle({ title, containerClass, icon, menuItems }) {
    return (
        <div className={classNames(containerClass)}>
            {typeof title === 'string' ? <h4 className="header-title">{title}</h4> : title}
            <Dropdown>
                <Dropdown.Toggle as={Link} to="#" className="arrow-none card-drop">
                    <i className={classNames(icon ? icon : 'mdi mdi-dots-vertical')} />
                </Dropdown.Toggle>
                <Dropdown.Menu align="end">
                    {(menuItems || []).map((item, index) => {
                        return (
                            <React.Fragment key={index}>
                                {item.hasDivider && <Dropdown.Divider as="div" />}
                                <Dropdown.Item className={classNames(item.variant ? item.variant : '')}>
                                    {item.icon && <i className={classNames(item.icon, 'me-1')}></i>}
                                    {item.label}
                                </Dropdown.Item>
                            </React.Fragment>
                        );
                    })}
                </Dropdown.Menu>
            </Dropdown>
        </div>
    );
}

CardTitle.propTypes = {
    menuItems: PropTypes.string,
    title: PropTypes.string,
    containerClass: PropTypes.string,
    icon: PropTypes.string,
    label: PropTypes.string,
    variant: PropTypes.string,
    hasDivider: PropTypes.bool,
};

export default CardTitle;
