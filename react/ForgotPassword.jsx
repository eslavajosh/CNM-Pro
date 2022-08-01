import { React, useState } from 'react';
import debug from 'sabio-debug';
import './forgotpassword.css';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import forgotPassSchema from '../../schema/forgotpassword/forgotPassSchema';
import userService from '../../services/userService';
import toastr from 'toastr';
import { useNavigate, Link } from 'react-router-dom';
import { Button } from 'react-bootstrap';
import swal from '@sweetalert/with-react';

const _logger = debug.extend('ForgotPassword');

function ForgotPassword() {
    const [forgotPassword] = useState({
        formData: { email: '' },
    });

    _logger('forgotPassword state', forgotPassword);
    const navigate = useNavigate();

    const onSubmit = (value) => {
        _logger('Payload data:', value);
        userService.forgotPassword(value).then(onSubmitSuccess).catch(onSubmitError);
    };

    const onSubmitSuccess = (response) => {
        swal({
            buttons: {
                cancel: 'Close',
            },
            content: (
                <div className="text-center m-auto">
                    <h4 className="text-dark-50 text-center mt-4 fw-bold">
                        An email has been sent to you with instructions to reset your password.
                    </h4>
                </div>
            ),
            icon: 'success',
        }).then(navigate('/login'));
        _logger('response', response);
    };

    const onSubmitError = (error) => {
        toastr.error('An account has not been found with this email. Try again.');
        _logger('error', error);
    };

    return (
        <Formik
            onSubmit={onSubmit}
            initialValues={forgotPassword.formData}
            enableReinitialize={true}
            validateOnChange={true}
            validationSchema={forgotPassSchema}>
            {({ values, errors, touched }) => (
                <Form className="form-main" align="center">
                    <div className="mb-3">
                        <label htmlFor="exampleInputEmail1" className="form-label">
                            <h3>Reset your password</h3>
                        </label>
                        <div className="form-header" id="emailHelp">
                            Enter your email and we will send you instructions to reset your password.
                        </div>
                        <div align="left" className="form-text">
                            Email
                        </div>
                        <Field
                            type="email"
                            name="email"
                            className="form-control email-form"
                            placeholder="example@email.com"
                            autoComplete="Email"
                            value={values.email}
                        />
                        {touched.email && errors.email && <div>{errors.email}</div>}
                    </div>
                    <div>
                        &nbsp;
                        <ErrorMessage name="Email" component="span" className="text-muted">
                            You must enter a valid email.
                        </ErrorMessage>
                    </div>
                    <Button type="submit" className="submit-button">
                        Submit
                    </Button>
                    <Link to="/register">
                        <div align="center" className="form-text-under">
                            Need an account? Sign up here
                        </div>
                    </Link>
                </Form>
            )}
        </Formik>
    );
}

export default ForgotPassword;
