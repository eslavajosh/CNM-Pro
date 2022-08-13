import React, { useEffect, useState } from 'react';
import blogService from '../../../services/blogService';
import logger from 'sabio-debug';
const _logger = logger.extend('Blog');

function LatestBlog() {
    const [blogFormData, setBlogFormData] = useState({
        title: '',
        subject: '',
        imageUrl: '',
        content: '',
        datePublish: '',
    });

    useEffect(() => {
        blogService.getAllBlogs(0, 1).then(onGetBlogSuccess).catch(onGetBlogError);
    }, []);

    const onGetBlogSuccess = (response) => {
        _logger('blog response success', response);

        const blogResponse = response.item.pagedItems[0];

        setBlogFormData((prevState) => {
            const newState = { ...prevState };
            newState.title = blogResponse.title;
            newState.subject = blogResponse.subject;
            newState.imageUrl = blogResponse.imageUrl;
            newState.content = blogResponse.content;
            newState.datePublish = blogResponse.datePublish;

            return newState;
        });

        _logger('new state', blogFormData);
    };

    const onGetBlogError = (error) => {
        _logger('blog response error', error);
    };

    return (
        <div className="card mb-3">
            <h4 className="header-title mb-3" align="left">
                Latest Blog
            </h4>
            <div className="row g-0">
                <div className="col-md-4">
                    <img src={blogFormData.imageUrl} className="img-fluid rounded-start" alt="..." />
                </div>
                <div className="col-md-8">
                    <div className="card-body">
                        <h5 className="card-title">{blogFormData.title}</h5>
                        <p className="card-text">{blogFormData.content}</p>
                        <p className="card-text">
                            <small className="text-muted">Published {blogFormData.datePublish}</small>
                        </p>
                        <a href="#" className="btn btn-primary">
                            Read more
                        </a>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default LatestBlog;
