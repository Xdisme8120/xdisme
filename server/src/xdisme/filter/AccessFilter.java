package xdisme.filter;

import java.io.IOException;
import java.io.PrintWriter;

import javax.servlet.Filter;
import javax.servlet.FilterChain;
import javax.servlet.FilterConfig;
import javax.servlet.ServletException;
import javax.servlet.ServletRequest;
import javax.servlet.ServletResponse;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

public class AccessFilter implements Filter{

    @Override
    public void destroy() {
        System.out.println("destory");
    }

    @Override
    public void doFilter(ServletRequest servletRequest, ServletResponse servletResponse,FilterChain filterChain) throws IOException, ServletException {
        System.out.println("filter");
        HttpServletRequest request = (HttpServletRequest) servletRequest;
        HttpServletResponse response = (HttpServletResponse) servletResponse;
        String method = request.getMethod();
        System.out.println(method);

        if ("GET".equals(method)) {
            System.out.println("GET");
            filterChain.doFilter(request, response);
            return;
        }else {
            System.out.println("post");
            filterChain.doFilter(request, response);

            return;
        }
    }

    @Override
    public void init(FilterConfig arg0) throws ServletException {
        System.out.println("init");
    }

}