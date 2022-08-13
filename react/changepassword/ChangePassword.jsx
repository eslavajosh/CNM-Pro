import { React, useState, useEffect } from 'react';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import '../forgotpassword/forgotpassword.css';
import changePassSchema from '../../schema/changepassword/changePassSchema';
import { useNavigate, Link } from 'react-router-dom';
import logger from 'sabio-debug';
import userService from '../../services/userService';
import toastr from 'toastr';
import { Button } from 'react-bootstrap';
import swal from '@sweetalert/with-react';

const _logger = logger.extend('ChangePassword');

function ChangePassword() {
    const [password, setPassword] = useState({
        formData: { password: '', token: '', email: '' },
    });

    const navigate = useNavigate();
    //useEffect and setState for form data

    const urlParams = new URLSearchParams(window.location.search);
    const tokenParam = urlParams.get('token');
    const emailParam = urlParams.get('email');

    _logger('url Params', tokenParam, emailParam);

    useEffect(() => {
        setPassword((prevState) => {
            const sd = { ...prevState };
            sd.formData.token = tokenParam;
            sd.formData.email = emailParam;
            return sd;
        });
    }, []);

    const onResetClicked = (value) => {
        _logger('onResetClicked', value);
        userService.changePassword(value).then(onResetSuccess).catch(onResetError);
    };

    const onResetSuccess = (response) => {
        swal({
            buttons: {
                cancel: 'Close',
            },
            content: (
                <div className="text-center m-auto">
                    <h4 className="text-dark-50 text-center mt-4 fw-bold">Password changed!</h4>
                    <h4 className="text-dark-50 text-center mt-4 fw-bold">
                        You will now be redirected to the login page.
                    </h4>
                </div>
            ),
            icon: 'success',
        }).then(navigate('/login'));
        _logger('Password Changed', response);
    };

    const onResetError = (error) => {
        toastr.error('Password fields must match.');
        _logger('error', error);
    };

    return (
        <Formik
            enableReinitialize={true}
            initialValues={password.formData}
            onSubmit={onResetClicked}
            validationSchema={changePassSchema}
            validateOnChange={true}>
            {({ values, errors, touched }) => (
                <Form className="form-main" align="center">
                    <div className="mb-3">
                        <label htmlFor="exampleInputEmail1" className="form-label">
                            <h3>Change Password</h3>
                        </label>
                        <div className="form-header" id="emailHelp">
                            Minimum of 8 characters, 1 lowercase letter, 1 uppercase letter, 1 number, 1 special
                            character ( !, @, #, $, %, ^, &, or *)
                        </div>
                        <div align="left" className="form-text">
                            Password
                        </div>
                        <Field
                            type="password"
                            name="password"
                            className="form-control password-form"
                            placeholder="Password"
                            autoComplete="Password"
                            value={values.password}
                        />
                        {touched.password && errors.password && <div className="text-danger">{errors.password}</div>}
                        <div>
                            &nbsp;
                            <ErrorMessage name="Email" component="span" className="text-muted">
                                You must enter a valid password.
                            </ErrorMessage>
                        </div>
                        <div align="left" className="form-text">
                            Confirm Password
                        </div>
                        <Field
                            type="password"
                            name="confirmpassword"
                            className="form-control password-form"
                            placeholder="Confirm Password"
                            autoComplete="Confirm Password"
                        />
                        <div>
                            &nbsp;
                            <ErrorMessage name="password" component="span" className="text-muted">
                                Passwords must match.
                            </ErrorMessage>
                        </div>
                        <Button type="submit" className="submit-button">
                            Reset
                        </Button>
                        <Link to="/contact">
                            <div align="center" className="form-text-under">
                                Need help? Contact us
                            </div>
                        </Link>
                    </div>
                </Form>
            )}
        </Formik>
    );
}

export default ChangePassword;
